using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace TwinFinder.Webservice
{
    public class Authenticator
    {
        private readonly object syncRoot = new object();

        private static readonly Authenticator instance = new Authenticator();

        private Cryptographer cryptographer;

        private const string alg = "HmacSHA256";
        private const string salt = "rz8LuOtFBXphj9WQfvFh";
        private int expirationMinutes = 0;
        private string userName = ConfigurationManager.AppSettings["twinfinder:Username"];
        private string password = ConfigurationManager.AppSettings["twinfinder:Password"];

        private readonly Dictionary<string, AuthenticationToken> sessions = new Dictionary<string, AuthenticationToken>();

        public static Authenticator Instance
        {
            get
            {
                return instance;
            }
        }

        public Authenticator()
        {
            var expirationString = ConfigurationManager.AppSettings["twinfinder:SessionExpirationMinutes"];
            int.TryParse(expirationString, out this.expirationMinutes);
        }

        public void Initialize()
        {
            // generate random bytes
            var key = new byte[32];
            var random = new Random();
            random.NextBytes(key);

            this.cryptographer = new Cryptographer(key,
                                                   new byte[] { 60, 33, 122, 108, 176, 163, 161, 139, 151, 145, 56, 162, 99, 161, 250, 141 });
        }

        public string CreateSession(string userName, string password)
        {
            if (userName == this.userName && password == this.password)
            {
                var token = new AuthenticationToken(userName, this.GetHashedPassword(password));
                string sessionKey = this.cryptographer.Encrypt<AuthenticationToken>(token);

                lock (this.syncRoot)
                {
                    if (!this.sessions.ContainsKey(sessionKey))
                    {
                        this.sessions.Add(sessionKey, token);
                    }
                }

                return sessionKey;
            }
            else
            {
                return "";
            }
        }

        public bool IsValidSession(string sessionKey)
        {
            string msg = this.ValidateSession(sessionKey);

            return string.IsNullOrEmpty(msg)
                        ? true
                        : false;
        }

        public string ValidateSession(string sessionKey)
        {
            string msg = "";
            try
            {
                var token = this.cryptographer.Decrypt<AuthenticationToken>(sessionKey);

                if (token.IsExpired(this.expirationMinutes))
                {
                    msg = string.Format("ERROR: The sessionkey has expired (Expiration time is {0} minutes)", this.expirationMinutes);
                }

                if (!this.sessions.ContainsKey(sessionKey))
                {
                    msg = "ERROR: A session with this key is not registred or has expired";
                }

                if (!this.sessions[sessionKey].Equals(token))
                {
                    msg = "ERROR: The sessionkey is not valid";
                }
            }
            catch (Exception ex)
            {
                msg = "ERROR: The sessionkey is not valid";
            }

            return msg;
        }

        private void CleanupExpiredTokens()
        {
            var toRemove = new List<string>();

            lock (this.syncRoot)
            {
                foreach (var session in this.sessions)
                {
                    var token = session.Value;
                    if (token.IsExpired(this.expirationMinutes))
                    {
                        toRemove.Add(session.Key);
                    }
                }

                foreach (var item in toRemove)
                {
                    if (this.sessions.ContainsKey(item))
                    {
                        this.sessions.Remove(item);
                    }
                }
            }
        }

        public void CloseSession(string sessionKey)
        {
            lock (this.syncRoot)
            {
                if (this.sessions.ContainsKey(sessionKey))
                {
                    this.sessions.Remove(sessionKey);
                }
            }
        }

        private byte[] GetHashedPassword(string password)
        {
            string key = string.Join(":", new { password, salt });

            using (HMAC hmac = HMACSHA256.Create(alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));

                return hmac.Hash;
            }
        }
    }
}
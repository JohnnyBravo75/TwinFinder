using System.Linq;

namespace TwinFinder.Webservice
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
#if !SILVERLIGHT
    [Serializable]
#endif
    public class AuthenticationToken
    {
        public AuthenticationToken()
        {
            this.CreationDateUtc = DateTime.UtcNow;
            this.ClientId = Guid.NewGuid();
        }

        public AuthenticationToken(string userName, byte[] passwordHash, string data = null)
        {
            this.UserName = userName;
            this.Data = data;
            this.PasswordHash = passwordHash;
            this.CreationDateUtc = DateTime.UtcNow;
            this.ClientId = Guid.NewGuid();
        }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime CreationDateUtc { get; set; }

        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public byte[] PasswordHash { get; set; }

        [DataMember]
        public Guid ClientId { get; set; }

        public bool Equals(AuthenticationToken other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.UserName == other.UserName &&
                (this.PasswordHash != null && this.PasswordHash.SequenceEqual(other.PasswordHash)) &&
                this.ClientId == other.ClientId &&
                this.CreationDateUtc == other.CreationDateUtc)
            {
                return true;
            }

            return false;
        }

        public bool IsExpired(int expirationMinutes)
        {
            if (expirationMinutes <= 0)
            {
                return false;
            }

            return Math.Abs((DateTime.UtcNow - this.CreationDateUtc).TotalMinutes) > expirationMinutes;
        }
    }
}
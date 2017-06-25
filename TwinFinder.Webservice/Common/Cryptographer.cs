namespace TwinFinder.Webservice
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Cryptographer : IDisposable
    {
        private readonly byte[] defaultInitVector = { 60, 33, 122, 108, 176, 163, 161, 139, 151, 145, 56, 162, 99, 161, 250, 141 };

        private readonly AesManaged aes = new AesManaged();

        private readonly ICryptoTransform decryptor;

        private readonly ICryptoTransform encryptor;

        private static readonly object lockObjectEncrypt = new object();

        private static readonly object lockObjectDecrypt = new object();

        public Cryptographer(byte[] key, byte[] initVector = null)
        {
            if (initVector == null)
            {
                initVector = this.defaultInitVector;
            }

            this.encryptor = this.aes.CreateEncryptor(key, initVector);
            this.decryptor = this.aes.CreateDecryptor(key, initVector);
        }

        public string Encrypt(string text)
        {
            var sourceBytes = Encoding.UTF8.GetBytes(text);

            var encrypted = this.Encrypt(sourceBytes);

            return encrypted;
        }

        public string Encrypt<T>(T input)
        {
            var sourceBytes = input.Serialize();
            var encrypted = this.Encrypt(sourceBytes);
            return encrypted;
        }

        public T Decrypt<T>(string encryptedString)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedString);
            var decryptedBytes = this.Decrypt(encryptedBytes);
            var result = decryptedBytes.Deserialize<T>();
            return result;
        }

        public string Decrypt(string encrypted)
        {
            var encryptedBytes = Convert.FromBase64String(encrypted);
            var decrypted = string.Empty;

            var decryptedBytes = this.Decrypt(encryptedBytes);

            decrypted = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);

            return decrypted;
        }

        private string Encrypt(byte[] sourceBytes)
        {
            lock (lockObjectEncrypt)
            {
                using (var memStream = new MemoryStream())
                {
                    using (var encryptStream = new CryptoStream(memStream, this.encryptor, CryptoStreamMode.Write))
                    {
                        encryptStream.Write(sourceBytes, 0, sourceBytes.Length);
                        encryptStream.FlushFinalBlock();

                        var encrypted = Convert.ToBase64String(memStream.ToArray());
                        memStream.Close();
                        encryptStream.Close();
                        return encrypted;
                    }
                }
            }
        }

        private byte[] Decrypt(byte[] encryptedBytes)
        {
            lock (lockObjectDecrypt)
            {
                var tmpBytes = new byte[encryptedBytes.Length - 1];

                using (var memStream = new MemoryStream(encryptedBytes))
                {
                    using (var cryptoStream = new CryptoStream(memStream, this.decryptor, CryptoStreamMode.Read))
                    {
                        var count = cryptoStream.Read(tmpBytes, 0, tmpBytes.Length);
                        memStream.Close();
                        cryptoStream.Close();

                        var decryptedBytes = new byte[count];
                        for (var i = 0; i < count; i++)
                        {
                            decryptedBytes[i] = tmpBytes[i];
                        }

                        return decryptedBytes;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (this.encryptor != null)
            {
                this.encryptor.Dispose();
            }

            if (this.decryptor != null)
            {
                this.decryptor.Dispose();
            }
        }
    }
}
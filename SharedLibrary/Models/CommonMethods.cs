using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SharedLibrary.Models
{
    public class CommonMethods
    {
        public static class EncryptDecryptKeyForCrypto
        {
            public readonly static string Key = "mySuperSecretKey123!@#";
        }

        public static string DecryptCrypto(string cipherText)
        {
            // Decode base64
            byte[] encryptedBytesWithSalt = Convert.FromBase64String(cipherText);

            // Check for "Salted__" header (8 bytes)
            byte[] saltHeader = new byte[8];
            Buffer.BlockCopy(encryptedBytesWithSalt, 0, saltHeader, 0, 8);
            string saltMarker = Encoding.ASCII.GetString(saltHeader);
            if (saltMarker != "Salted__")
                throw new Exception("Invalid encrypted text format");

            // Extract salt (next 8 bytes)
            byte[] salt = new byte[8];
            Buffer.BlockCopy(encryptedBytesWithSalt, 8, salt, 0, 8);

            // Extract actual encrypted data
            byte[] encryptedBytes = new byte[encryptedBytesWithSalt.Length - 16];
            Buffer.BlockCopy(encryptedBytesWithSalt, 16, encryptedBytes, 0, encryptedBytes.Length);

            // Derive key and IV using OpenSSL's EVP_BytesToKey (MD5-based)
            byte[] key, iv;
            DeriveKeyAndIV(Encoding.UTF8.GetBytes(EncryptDecryptKeyForCrypto.Key), salt, out key, out iv);

            // Decrypt
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(encryptedBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        // Implements OpenSSL EVP_BytesToKey with MD5
        private static void DeriveKeyAndIV(byte[] password, byte[] salt, out byte[] key, out byte[] iv)
        {
            var keyLength = 32; // AES-256
            var ivLength = 16;  // AES block size
            var d = new List<byte>();
            byte[] prev = Array.Empty<byte>();

            using (var md5 = MD5.Create())
            {
                while (d.Count < keyLength + ivLength)
                {
                    byte[] input = new byte[prev.Length + password.Length + salt.Length];
                    Buffer.BlockCopy(prev, 0, input, 0, prev.Length);
                    Buffer.BlockCopy(password, 0, input, prev.Length, password.Length);
                    Buffer.BlockCopy(salt, 0, input, prev.Length + password.Length, salt.Length);

                    prev = md5.ComputeHash(input);
                    d.AddRange(prev);
                }
            }

            key = d.Take(keyLength).ToArray();
            iv = d.Skip(keyLength).Take(ivLength).ToArray();
        }


        public static class EncryptDecryptKey
        {
            public readonly static string Key = "superSecretKey123!@#";
        }

        //For backend 
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = EncryptDecryptKey.Key;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Dispose();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        //For backend
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = EncryptDecryptKey.Key;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Dispose();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}

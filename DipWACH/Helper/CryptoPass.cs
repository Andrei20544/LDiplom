using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DipWACH.Helper
{
    public static class CryptoPass
    {
        private static byte[] _keys = new byte[16] { 9, 152, 89, 173, 133, 101, 235, 27, 39, 173, 62, 187, 245, 85, 61, 66 };

        private static UnicodeEncoding _byteConverter = new UnicodeEncoding();
        private static RSACryptoServiceProvider _rSA = new RSACryptoServiceProvider();

        public static string GetHashPassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                byte[] salt = new byte[128 / 8];

                for (int i = 0; i < salt.Length; i++) salt[i] = _keys[i];

                //using (var rngCsp = new RNGCryptoServiceProvider())
                //{
                //    rngCsp.GetNonZeroBytes(salt);
                //}
                //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                

                return hashed;
            }

            return null;
        }

        public static string GetEncryptPassword(string password)
        {
            try
            {
                byte[] passbyte = _byteConverter.GetBytes(password);

                byte[] encryptPasswordByte;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    RSA.ImportParameters(_rSA.ExportParameters(false));
                    encryptPasswordByte = RSA.Encrypt(passbyte, false);

                }

                string encryptPassword = _byteConverter.GetString(encryptPasswordByte);

                return encryptPassword;
            }
            catch(CryptographicException e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        public static string GetDecryptPassword(string password)
        {
            try
            {
                byte[] passbyte = _byteConverter.GetBytes(password);

                byte[] decryptPasswordByte;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(_rSA.ExportParameters(true));
                    decryptPasswordByte = RSA.Decrypt(passbyte, false);
                }

                string decryptPassword = _byteConverter.GetString(decryptPasswordByte);

                return decryptPassword;
            }
            catch (CryptographicException e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
    }
}

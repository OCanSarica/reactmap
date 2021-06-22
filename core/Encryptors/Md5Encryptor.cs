using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using core.Bases;

namespace core.Encryptors
{
    internal class Md5Encryptor : IEncryptor
    {
        private readonly string _Key;

        public Md5Encryptor(string _key)
        {
            _Key = _key;
        }

        public string Encrypt(string _data)
        {
            var _utf8 = new UTF8Encoding();

            var _md5 = new MD5CryptoServiceProvider();

            var _deskey = _md5.ComputeHash(_utf8.GetBytes(_Key));

            var _desalg = new TripleDESCryptoServiceProvider
            {
                Key = _deskey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
            };

            var _encrptData = _utf8.GetBytes(_data);

            byte[] _result;

            try
            {
                var _encryptor = _desalg.CreateEncryptor();

                _result = _encryptor.TransformFinalBlock(_encrptData, 0, _encrptData.Length);
            }
            finally
            {
                _desalg.Clear();

                _md5.Clear();
            }

            return Convert.ToBase64String(_result);
        }

        public string Decrypt(string _data)
        {
            var _utf8 = new UTF8Encoding();

            var _md5 = new MD5CryptoServiceProvider();

            var _deskey = _md5.ComputeHash(_utf8.GetBytes(_Key));

            var _desalg = new TripleDESCryptoServiceProvider
            {
                Key = _deskey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
            };

            var _decryptData = Convert.FromBase64String(_data);

            byte[] _result;

            try
            {
                var _decryptor = _desalg.CreateDecryptor();

                

                _result = _decryptor.TransformFinalBlock(_decryptData, 0, _decryptData.Length);
            }
            catch(Exception _ex)
            {
                throw;
            }
            finally
            {
                _desalg.Clear();

                _md5.Clear();
            }

            return _utf8.GetString(_result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using core.Bases;

namespace core.Encryptors
{
    internal class RsaEncryptor : IEncryptor
    {
        private readonly string _PrivateKey;
        
        private readonly string _PublicKey;

        public RsaEncryptor(string _privateKey, string _publicKey)
        {
            _PrivateKey = _privateKey;

            _PublicKey = _publicKey;
        }

        public string Encrypt(string _data)
        {
            var _result = "";

            try
            {
                using var _rsa = new RSACryptoServiceProvider();

                _rsa.FromXmlString(_PublicKey);

                var _dataToEncrypt = Encoding.UTF8.GetBytes(_data);

                var _encryptedData = _rsa.Encrypt(_dataToEncrypt, false);

                _result = Convert.ToBase64String(_encryptedData);
            }
            catch (Exception _ex)
            {
            }

            return _result;
        }

        public string Decrypt(string _data)
        {
            var _result = "";

            try
            {
                using var _rsa = new RSACryptoServiceProvider();

                _rsa.FromXmlString(_PrivateKey);

                var _dataToDecrypt = Convert.FromBase64String(_data);

                var _decryptedData = _rsa.Decrypt(_dataToDecrypt, false);

                _result = Encoding.UTF8.GetString(_decryptedData);
            }
            catch (Exception _ex)
            {
            }

            return _result;
        }
    }
}

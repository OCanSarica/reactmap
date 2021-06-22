using System;
using System.Collections.Generic;
using System.Text;

namespace core.Bases
{
    interface IEncryptor
    {
        public string Encrypt(string _value);

        public string Decrypt(string _value);
    }
}

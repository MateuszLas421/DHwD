using DHwD.Interface;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DHwD.Model
{
    public class Hash: IHash
    {
        public string ComputeHash(string input, HashAlgorithm algorithm)  //Hash
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DHwD.Interface
{
    interface IHash
    {
        string ComputeHash(string input, HashAlgorithm algorithm);
    }
}

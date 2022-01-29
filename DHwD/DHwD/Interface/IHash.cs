using System.Security.Cryptography;

namespace DHwD.Interface
{
    interface IHash
    {
        string ComputeHash(string input, HashAlgorithm algorithm);
    }
}

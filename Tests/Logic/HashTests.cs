using System;
using System.Security.Cryptography;
using Xunit;

namespace Tests.Logic
{
    public class HashTests
    {
        public HashTests()
        {

        }
        [InlineData("tafsafsfsaasf")]
        [InlineData("%$$#^@$^&$%&^$%&$#@%$#%#$FREGVRGS$^#$^$#@%$")]
        [InlineData("")]
        [Theory]
        public void Hash(string testText)
        {
            DHwD.Models.Hash hash = new DHwD.Models.Hash();
            var hash1 = hash.ComputeHash(testText, new SHA256CryptoServiceProvider());
            hash = new DHwD.Models.Hash();
            var hash2 = hash.ComputeHash(testText, new SHA256CryptoServiceProvider());
            Assert.Equal(hash1, hash2);
        }
    }
}

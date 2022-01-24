using DHwD.Tools;
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
            Hash hash = new Hash();
            var hash1 = hash.ComputeHash(testText, new SHA256CryptoServiceProvider());
            hash = new Hash();
            var hash2 = hash.ComputeHash(testText, new SHA256CryptoServiceProvider());
            Assert.Equal(hash1, hash2);
        }
    }
}

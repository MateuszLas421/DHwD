using System;
using DHwD.Repository.Interfaces;
using Xunit;

namespace Tests.Logic.DI
{
    public class LogsTests
    {
        ILogs _logs;
        public LogsTests(ILogs logs)
        {
            _logs = logs;
        }

        [Fact]
        public void NewExeption()
        {
            try
            {
                _logs.LogError(new Exception("Test Exception"));
            }
            catch (Exception)
            {
                Assert.True(false);
            }
            Assert.True(true);
        }
    }
}

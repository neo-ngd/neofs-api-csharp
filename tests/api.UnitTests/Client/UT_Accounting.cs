using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoFS.API.v2.Cryptography;
using System;

namespace NeoFS.API.v2.UnitTests.FSClient
{
    [TestClass]
    public class UT_Accounting
    {
        [TestMethod]
        public void TestBalance()
        {
            var host = "localhost:8080";
            var key = "L4kWTNckyaWn2QdUrACCJR1qJNgFFGhTCy63ERk7ZK3NvBoXap6t".LoadWif();
            var client = new Client.Client(host, key);
            var balance = client.GetSelfBalance();
            Assert.AreEqual(0, balance.Value);
        }
    }
}

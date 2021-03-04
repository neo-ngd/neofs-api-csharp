using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Cryptography;
using System;

namespace Neo.FileSystem.API.UnitTests.FSClient
{
    [TestClass]
    public class UT_Accounting
    {
        [TestMethod]
        public void TestBalance()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            Console.WriteLine(key.ToAddress_obsolete());
            var client = new Client.Client(key, host);
            var balance = client.GetSelfBalance().Result;
            Assert.AreEqual(0, balance.Value);
        }
    }
}

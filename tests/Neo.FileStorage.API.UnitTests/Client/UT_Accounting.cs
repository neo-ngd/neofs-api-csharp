using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public class UT_Accounting
    {
        [TestMethod]
        public void TestBalance()
        {
            var host = "http://localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(key, host);
            var balance = client.GetBalance().Result;
            Assert.AreEqual(0, balance.Value);
        }
    }
}

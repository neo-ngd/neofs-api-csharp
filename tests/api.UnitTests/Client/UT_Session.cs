using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo;
using NeoFS.API.v2.Cryptography;
using System;
using System.Threading;

namespace NeoFS.API.v2.UnitTests.FSClient
{
    [TestClass]
    public class UT_Session
    {
        [TestMethod]
        public void TestSessionCreate()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(host, key);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var token = client.CreateSession(source.Token, ulong.MaxValue);
            Assert.AreEqual(key.ToOwnerID(), token.Body.OwnerId);
            Console.WriteLine($"id={token.Body.Id.ToUUID()}, key={token.Body.SessionKey.ToByteArray().ToHexString()}");
        }
    }
}

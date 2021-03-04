using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Cryptography;
using System;
using System.Threading;

namespace Neo.FileSystem.API.UnitTests.FSClient
{
    [TestClass]
    public class UT_Session
    {
        [TestMethod]
        public void TestSessionCreate()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
<<<<<<< HEAD
            var token = client.CreateSession(source.Token, ulong.MaxValue);
            Assert.AreEqual(key.ToOwnerID_obsolete(), token.Body.OwnerId);
=======
            var token = client.CreateSession(source.Token, ulong.MaxValue).Result;
            Assert.AreEqual(key.ToOwnerID(), token.Body.OwnerId);
>>>>>>> master
            Console.WriteLine($"id={token.Body.Id.ToUUID()}, key={token.Body.SessionKey.ToByteArray().ToHexString()}");
        }
    }
}

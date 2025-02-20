using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestSessionCreate()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(10));
            var token = client.CreateSession(ulong.MaxValue, context: source.Token).Result;
            Console.WriteLine(token.ToString());
            Assert.AreEqual(key.OwnerID(), token.Body.OwnerId);
            Console.WriteLine($"id={token.Body.Id.ToUUID()}, key={token.Body.SessionKey.ToByteArray().ToHexString()}");
        }
    }
}

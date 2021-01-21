using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoFS.API.v2.Cryptography;
using System;
using System.Threading;

namespace NeoFS.API.v2.UnitTests.FSClient
{
    [TestClass]
    public class UT_Netmap
    {
        [TestMethod]
        public void TestLocalNodeInfo()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(host, key);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var ni = client.LocalNodeInfo(source.Token);
            Console.WriteLine(ni.ToString());
        }

        [TestMethod]
        public void TestEpoch()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(host, key);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var epoch = client.Epoch(source.Token);
            Console.WriteLine(epoch);
        }
    }
}

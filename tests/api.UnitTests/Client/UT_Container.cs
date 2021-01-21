using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoFS.API.v2.Acl;
using NeoFS.API.v2.Cryptography;
using NeoFS.API.v2.Netmap;
using NeoFS.API.v2.Refs;
using System;
using System.Threading;

namespace NeoFS.API.v2.UnitTests.FSClient
{
    [TestClass]
    public class UT_Container
    {
        [TestMethod]
        public void TestPutContainer()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(host, key);
            var replica = new Replica(1, "*");
            var policy = new PlacementPolicy(2, new Replica[] { replica }, null, null);
            var container = new Container.Container
            {
                Version = Refs.Version.SDKVersion(),
                OwnerId = key.ToOwnerID(),
                Nonce = new Guid().ToByteString(),
                BasicAcl = (uint)BasicAcl.PublicBasicRule,
                PlacementPolicy = policy,
            };
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var cid = client.PutContainer(source.Token, container);
            Console.WriteLine(cid.ToBase58String());
            Assert.AreEqual(container.CalCulateAndGetID, cid);
        }

        [TestMethod]
        public void TestGetContainer()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(host, key);
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var container = client.GetContainer(source.Token, cid);
            Assert.AreEqual(cid, container.CalCulateAndGetID);
        }

        [TestMethod]
        public void TestDeleteContainer()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(host, key);
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.DeleteContainer(source.Token, cid);
        }

        [TestMethod]
        public void TestListContainer()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(host, key);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var cids = client.ListContainers(source.Token, key.ToOwnerID());
            Assert.AreEqual(1, cids.Count);
            Assert.AreEqual("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz", cids[0].ToBase58String());
        }

        [TestMethod]
        public void TestGetExtendedACL()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var client = new Client.Client(host, key);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var eacl = client.GetEACL(source.Token, cid);
            Console.WriteLine(eacl);
        }

        [TestMethod]
        public void TestSetExtendedACL()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var client = new Client.Client(host, key);
            var eacl = new EACLTable
            {
                Version = Refs.Version.SDKVersion(),
                ContainerId = cid,
            };
            var filter = new EACLRecord.Types.Filter
            {
                HeaderType = HeaderType.HeaderUnspecified,
                MatchType = MatchType.StringEqual,
                Key = "test",
                Value = "test"
            };
            var target = new EACLRecord.Types.Target
            {
                Role = Role.Others,
            };
            var record = new EACLRecord
            {
                Operation = Acl.Operation.Get,
                Action = Acl.Action.Deny,
            };
            record.Filters.Add(filter);
            record.Targets.Add(target);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.SetEACL(source.Token, eacl);
        }
    }
}

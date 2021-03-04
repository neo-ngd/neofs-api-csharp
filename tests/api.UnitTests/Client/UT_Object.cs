using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Client;
using Neo.FileSystem.API.Client.ObjectParams;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Refs;
using Neo.FileSystem.API.Object;
using V2Object = Neo.FileSystem.API.Object.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace Neo.FileSystem.API.UnitTests.FSClient
{
    [TestClass]
    public class UT_Object
    {
        [TestMethod]
        public void TestObjectPut()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("BERrKi1LRXGy1cHMhssxa4wWuHCdkYYGXBzdLGmmAJLK");
            var payload = Encoding.ASCII.GetBytes("hello");
            var obj = new V2Object
            {
                Header = new Header
                {
                    OwnerId = key.ToOwnerID_obsolete(),
                    ContainerId = cid,
                },
                Payload = ByteString.CopyFrom(payload),
            };
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(source1.Token, ulong.MaxValue).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.PutObject(source2.Token, new PutObjectParams { Object = obj }, new CallOptions { Ttl = 2, Session = session }).Result;
            Console.WriteLine(o.ToBase58String());
            Assert.AreNotEqual("", o.ToBase58String());
        }

        [TestMethod]
        public void TestObjectGet()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("RuzuV3RDstuVtWoDzsTsuNFiakaaGGN24EbNSUFGaiQ");
            var oid = ObjectID.FromBase58String("6VLqsZAvYTRzt8yY4NvGweWfGmqBiAfQwd6novRNFYiG");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(source1.Token, ulong.MaxValue).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObject(source2.Token, new GetObjectParams { Address = address }, new CallOptions { Ttl = 2, Session = session }).Result;
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectGetWithoutOptions()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("RuzuV3RDstuVtWoDzsTsuNFiakaaGGN24EbNSUFGaiQ");
            var oid = ObjectID.FromBase58String("6VLqsZAvYTRzt8yY4NvGweWfGmqBiAfQwd6novRNFYiG");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObject(source.Token, new GetObjectParams { Address = address }).Result;
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectDelete()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var oid = ObjectID.FromBase58String("CnBNgUmXiA5KJeGvMDgUEGiKrbZctjwxT5v3sBYjnmf1");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(source1.Token, ulong.MaxValue).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.DeleteObject(source2.Token, new DeleteObjectParams { Address = address }, new CallOptions { Ttl = 2, Session = session });
            Assert.AreEqual(address, o);
        }

        [TestMethod]
        public void TestObjectHeaderGet()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var oid = ObjectID.FromBase58String("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(source1.Token, ulong.MaxValue).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectHeader(source2.Token, new ObjectHeaderParams { Address = address, Short = false }, new CallOptions { Ttl = 2, Session = session }).Result;
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectGetRange()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var oid = ObjectID.FromBase58String("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(source1.Token, ulong.MaxValue).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeData(source2.Token, new RangeDataParams { Address = address, Range = new Object.Range { Offset = 0, Length = 3 } }, new CallOptions { Ttl = 2, Session = session }).Result;
            Assert.AreEqual("hel", Encoding.ASCII.GetString(o));
        }

        [TestMethod]
        public void TestObjectGetRangeHash()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var oid = ObjectID.FromBase58String("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(source1.Token, ulong.MaxValue).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeHash(source2.Token, new RangeChecksumParams { Address = address, Ranges = new List<Object.Range> { new Object.Range { Offset = 0, Length = 3 } }, Salt = new byte[] { 0x00 }, Type = ChecksumType.Sha256 }, new CallOptions { Ttl = 2, Session = session }).Result;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(Encoding.ASCII.GetBytes("hello")[..3].Sha256().ToHexString(), o[0].ToHexString());
        }

        [TestMethod]
        public void TestObjectSearch()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(source1.Token, ulong.MaxValue).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.SearchObject(source2.Token, new SearchObjectParams { ContainerID = cid, Filters = new SearchFilters() }, new CallOptions { Ttl = 2, Session = session }).Result;
            Console.WriteLine(o.Count);
            Assert.IsTrue(o.Select(p => p.ToBase58String()).ToList().Contains("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx"));
        }

        [TestMethod]
        public void TestClient()
        {
            var key = "7310c4da083264666cc3567d5cb4a5b060ca34fb68959de58bd04959f3cbc6b2".HexToBytes().LoadPrivateKey();
            var client = new Client.Client(key, "127.0.0.1:8080");
        }
    }
}

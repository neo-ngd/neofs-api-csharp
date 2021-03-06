using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using static Neo.FileStorage.API.Cryptography.KeyExtension;

namespace Neo.FileStorage.API.UnitTests.TestCryptography
{
    [TestClass]
    public class UT_Key
    {
        [TestMethod]
        public void TestPublicKeyDecompress()
        {
            var public_key = "0203592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c1".HexToBytes();
            Assert.AreEqual("0403592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c148f1d40b79783f97de100496226b2d378c5297ab766c0f07c2a8df6ec2deed30", public_key.Decompress().ToHexString());
        }

        [TestMethod]
        public void TestPublicKeyCompress()
        {
            var public_key = "0403592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c148f1d40b79783f97de100496226b2d378c5297ab766c0f07c2a8df6ec2deed30".HexToBytes();
            Assert.AreEqual("0203592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c1", public_key.Compress().ToHexString());
        }

        [TestMethod]
        public void TestOwnerId()
        {
            var address = "NTfozM1xX7WDKD2LUE5yjtd8FMFYQJoy54";
            var ownerId = address.AddressToOwnerID();
            Assert.AreEqual(25, ownerId.Value.Length);
            var key = "Kwk6k2eC3L3QuPvD8aiaNyoSXgQ2YL1bwS5CP1oKoA9waeAze97s".LoadWif();
            ownerId = key.ToOwnerID();
            Assert.AreEqual(25, ownerId.Value.Length);
            Assert.AreEqual(27, ownerId.ToByteArray().Length);
        }

        [TestMethod]
        public void TestPublicKey()
        {
            var key = "0203592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c1".HexToBytes().LoadPublicKey();
            Assert.AreEqual("0203592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c1", key.PublicKey().ToHexString());
            Assert.AreEqual("Nedo3Wtrx8fDjtYosjt7ZERHyR2EsgPtxK", key.ToAddress());
        }

        [TestMethod]
        public void TestWif1()
        {
            var key = "L4kWTNckyaWn2QdUrACCJR1qJNgFFGhTCy63ERk7ZK3NvBoXap6t".LoadWif();
            var address = key.ToAddress();
            Assert.AreEqual("Nivku7mFnqdzVftRnUYHo5hD1oJT71yeZp", address);
            key = "L1pBKpw4tR6CogySzye3GUcVPz5pAeemXbyupoWUEVrtfstBfDiN".LoadWif();
            Assert.AreEqual("NgYMvdwCWMypHLqoZ6uqYutbeAAAgbSFcD", key.ToAddress());
        }

        [TestMethod]
        public void TestWif2()
        {
            var key = "L4kWTNckyaWn2QdUrACCJR1qJNgFFGhTCy63ERk7ZK3NvBoXap6t".LoadWif();
            var private_key = key.ExportParameters(true).D;
            Assert.AreEqual("e0b48fb95d04aa475a0da759218a85d9b03cf4e55b79458dcdf4d42a7fe29cd1", private_key.ToHexString());
        }

        [TestMethod]
        public void TestWif3()
        {
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            Assert.AreEqual("NbUgTSFvPmsRxmGeWpuuGeJUoRoi6PErcM", key.ToAddress());
        }
    }
}

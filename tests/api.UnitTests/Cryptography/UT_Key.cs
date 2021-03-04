using System;
using System.Linq;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Cryptography;
using static Neo.FileSystem.API.Cryptography.Helper;
using static Neo.FileSystem.API.Cryptography.KeyExtension;

namespace Neo.FileSystem.API.UnitTests.TestCryptography
{
    [TestClass]
    public class UT_Key
    {
        [TestMethod]
        public void TestCreateSignatureRedeemScript()
        {
            var public_key = "0203592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c1".HexToBytes();
            Assert.AreEqual("0c210203592a65bd5fb116a3381f1f29a125bac8658cd592d2a8dc9fed886c891f16c141747476aa", public_key.CreateSignatureRedeemScript().ToHexString());
            Assert.AreEqual("967a501264e563ac81a6f7afbdb14949efb39a85", public_key.CreateSignatureRedeemScript().Sha256().RIPEMD160().ToHexString());
            Assert.AreEqual("NZdd4yJPMjjMTXT8eimx55it16wzWiji5C", public_key.CreateSignatureRedeemScript().Sha256().RIPEMD160().ToAddress(NeoAddressVersion));
        }

        [TestMethod]
        public void TestCreateSignatureRedeemScript_obsolete()
        {
            var public_key = "03051c088573c77eec3d9abad0bf292fb092bb1ba32eb1cde776572199a3bc2f95".HexToBytes();
            Assert.AreEqual("0c2103051c088573c77eec3d9abad0bf292fb092bb1ba32eb1cde776572199a3bc2f950b4195440d78", public_key.CreateSignatureRedeemScript_obsolete().ToHexString());
            Assert.AreEqual("NPQxTX185Cw6T8RxCk9E2CEiDYxpk972Uj", public_key.CreateSignatureRedeemScript_obsolete().Sha256().RIPEMD160().ToAddress(NeoAddressVersion));
        }

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
            Assert.AreEqual("NZdd4yJPMjjMTXT8eimx55it16wzWiji5C", key.ToAddress());
        }

        [TestMethod]
        public void TestWif1()
        {
            var key = "L4kWTNckyaWn2QdUrACCJR1qJNgFFGhTCy63ERk7ZK3NvBoXap6t".LoadWif();
            var address = key.ToAddress();
            Assert.AreEqual("NNpKztcTN2XVve1mQVtF3ckWKKjEUcoPSs", address);
            key = "L1pBKpw4tR6CogySzye3GUcVPz5pAeemXbyupoWUEVrtfstBfDiN".LoadWif();
            Assert.AreEqual("NQ8qB87zoN1P4qG74V9fkN9Zp7twis3g2r", key.ToAddress());
        }

        [TestMethod]
        public void TestWif2()
        {
            var key = "L4kWTNckyaWn2QdUrACCJR1qJNgFFGhTCy63ERk7ZK3NvBoXap6t".LoadWif();
            var private_key = key.ExportParameters(true).D;
            Assert.AreEqual("e0b48fb95d04aa475a0da759218a85d9b03cf4e55b79458dcdf4d42a7fe29cd1", private_key.ToHexString());
        }
    }
}

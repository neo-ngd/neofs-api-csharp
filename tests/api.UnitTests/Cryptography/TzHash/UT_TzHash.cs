using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Cryptography.Tz;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Neo.FileSystem.API.UnitTests.TestCryptography.Tz
{
    [TestClass]
    public class UT_TzHash
    {
        private readonly Tuple<byte[], string>[] HashTestCases = new[]
        {
            new Tuple<byte[], string>(
                new byte[]{},
                "00000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001"
                ),
            new Tuple<byte[], string>(
                new byte[]{0, 1, 2, 3, 4, 5, 6, 7, 8},
                "00000000000001e4a545e5b90fb6882b00000000000000c849cd88f79307f67100000000000000cd0c898cb68356e624000000000000007cbcdc7c5e89b16e4b"
                ),
            new Tuple<byte[], string>(
                new byte[]{4, 8, 15, 16, 23, 42, 255, 0, 127, 65, 32, 123, 42, 45, 201, 210, 213, 244},
                "4db8a8e253903c70ab0efb65fe6de05a36d1dc9f567a147152d0148a86817b2062908d9b026a506007c1118e86901b672a39317c55ee3c10ac8efafa79efe8ee"
                ),
        };

        private readonly Tuple<string, string[]>[] ConcatTestCases = new[]
        {
            new Tuple<string, string[]>(
                "7f5c9280352a8debea738a74abd4ec787f2c5e556800525692f651087442f9883bb97a2c1bc72d12ba26e3df8dc0f670564292ebc984976a8e353ff69a5fb3cb",
                new string[]{
                    "4275945919296224acd268456be23b8b2df931787a46716477e32cd991e98074029d4f03a0fedc09125ee4640d228d7d40d430659a0b2b70e9cd4d4c5361865a",
                    "2828661d1b1e77f21788d3b365f140a2395d57dc2083c33e60d9a80e69017d5016a249c7adfe1718a10ba887dedbdaec5c4c1fbecdb1f98776b43f1142c26a88",
                    "02310598b45dfa77db9f00eed6ab60773dd8bed7bdac431b42e441fae463f64c6e2688402cfdcec5def47a299b0651fb20878cf4410991bd57056d7b4b31635a",
                    "1ed7e0b065c060d915e7355cdcb4edc752c06d2a4b39d90c8985aeb58e08cb9e5bbe4b2b45524efbd68cd7e4081a1b8362941200a4c9f76a0a9f9ac9b7868c03",
                    "6f11e3dc4fff99ffa45e36e4655cfc657c29e950e598a90f426bf5710de9171323523db7636643b23892783f4fb3cf8e583d584c82d29558a105a615a668fc9e",
                    "1865dbdb4c849620fb2c4809d75d62490f83c11f2145abaabbdc9a66ae58ce1f2e42c34d3b380e5dea1b45217750b42d130f995b162afbd2e412b0d41ec8871b",
                    "5102dd1bd1f08f44dbf3f27ac895020d63f96044ce3b491aed3efbc7bbe363bc5d800101d63890f89a532427812c30c9674f37476ba44daf758afa88d4f91063",
                    "70cab735dad90164cc61f7411396221c4e549f12392c0d77728c89a9754f606c7d961169d4fa88133a1ba954bad616656c86f8fd1335a2f3428fd4dca3a3f5a5",
                    "430f3e92536ff9a50cbcdf08d8810a59786ca37e31d54293646117a93469f61c6cdd67933128407d77f3235293293ee86dbc759d12dfe470969eba1b4a373bd0",
                    "46e1d97912ca2cf92e6a9a63667676835d900cdb2fff062136a64d8d60a8e5aa644ccee3558900af8e77d56b013ed5da12d9d0b7de0f56976e040b3d01345c0d",
                })
        };

        private readonly Tuple<string, string, string>[] SubstractTestCases = new[]
        {
            new Tuple<string, string, string>(
                "4275945919296224acd268456be23b8b2df931787a46716477e32cd991e98074029d4f03a0fedc09125ee4640d228d7d40d430659a0b2b70e9cd4d4c5361865a",
                "277c10e0d7c52fcc0b23ba7dbf2c3dde7dcfc1f7c0cc0d998b2de504b8c1e17c6f65ab1294aea676d4060ed2ca18c1c26fd7cec5012ab69a4ddb5e6555ac8a59",
                "7f5c9280352a8debea738a74abd4ec787f2c5e556800525692f651087442f9883bb97a2c1bc72d12ba26e3df8dc0f670564292ebc984976a8e353ff69a5fb3cb"
                ),
            new Tuple<string, string, string>(
                "18e2ce290cc74998ebd0bef76454b52a40428f13bb612e40b5b96187e9cc813248a0ed5f7ec9fb205d55d3f243e2211363f171b19eb8acc7931cf33853a79069",
                "73a0582fa7d00d62fd09c1cd18589cdb2b126cb58b3a022ae47a8a787dabe35c4388aaf0d8bb343b1e58ee8d267812d115f40a0da611f42458f452e102f60700",
                "54ccaad1bb15b2989fa31109713bca955ea5d87bbd3113b3008cea167c00052266e9c9fcb73ece98c6c08cccb074ba3d39b5d8685f022fc388e2bf1997c5bd1d"
                )
        };

        [TestMethod]
        public void TestHash()
        {
            foreach (var item in HashTestCases)
            {
                TzHash tz = new TzHash();
                var hash = tz.ComputeHash(item.Item1);
                Assert.AreEqual(item.Item2, hash.ToHexString());
            }
        }

        [TestMethod]
        public void TestHomomorphism()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var b = new byte[64];
            rng.GetBytes(b);

            TzHash tz = new TzHash();
            var h = tz.ComputeHash(b);
            var s = h.ToHexString();
            tz.Reset();
            var h1 = tz.ComputeHash(b[0..32]);
            var s1 = h1.ToHexString();
            tz.Reset();
            var h2 = tz.ComputeHash(b[32..]);
            var s2 = h2.ToHexString();

            var r1 = new SL2().FromByteArray(h1);
            var r2 = new SL2().FromByteArray(h2);
            var r = r1 * r2;

            Assert.AreEqual(s, r.ToByteArray().ToHexString());
        }

        [TestMethod]
        public void TestConcat()
        {
            foreach (var item in ConcatTestCases)
            {
                var expected = item.Item1;
                var hashes = item.Item2.Select(p => p.HexToBytes()).ToList();
                var actual = TzHash.Concat(hashes);
                Assert.AreEqual(expected, actual.ToHexString());
            }
        }

        [TestMethod]
        public void TestValidate()
        {
            foreach (var item in ConcatTestCases)
            {
                var result = TzHash.Validate(item.Item1.HexToBytes(), item.Item2.Select(p => p.HexToBytes()).ToList());
                Assert.AreEqual(true, result);
            }
        }

        [TestMethod]
        public void TestSubstract()
        {
            foreach (var item in SubstractTestCases)
            {
                var r = TzHash.SubstractR(item.Item2.HexToBytes(), item.Item3.HexToBytes());
                Assert.AreEqual(item.Item1, r.ToHexString());

                var l = TzHash.SubstractL(item.Item1.HexToBytes(), item.Item3.HexToBytes());
                Assert.AreEqual(item.Item2, l.ToHexString());
            }
        }

        [TestMethod]
        public void TestCorrectness()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var b = new byte[1000];
            rng.GetBytes(b);

            TzHash tz = new TzHash();
            var h = tz.ComputeHash(b);
            var s = h.ToHexString();

            tz.Reset();
            tz.HashDataParallel(b);
            var h2 = tz.Hash;
            var s2 = h2.ToHexString();
            Assert.AreEqual(s, s2);
        }

        [TestMethod]
        public void TestSpeed1()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var b = new byte[10000000];
            rng.GetBytes(b);

            TzHash tz = new TzHash();
            var h = tz.ComputeHash(b);
            var s = h.ToHexString();
        }

        [TestMethod]
        public void TestSpeed2()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var b = new byte[100000];
            rng.GetBytes(b);

            TzHash tz = new TzHash();
            tz.HashDataParallel(b);
            var h2 = tz.Hash;
            var s2 = h2.ToHexString();
        }
    }
}

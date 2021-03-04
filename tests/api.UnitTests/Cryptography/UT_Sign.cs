using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Accounting;
using Neo.FileSystem.API.Session;

namespace Neo.FileSystem.API.UnitTests.TestCryptography
{
    [TestClass]
    public class UT_Sign
    {
        [TestMethod]
        public void TestSignData1()
        {
            var key = "Kwk6k2eC3L3QuPvD8aiaNyoSXgQ2YL1bwS5CP1oKoA9waeAze97s".LoadWif();

            var sig = key.SignData("024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d".HexToBytes());
            Assert.IsTrue(key.VerifyData("024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d".HexToBytes(), sig));

            var key1 = key.PublicKey().LoadPublicKey();
            Assert.IsTrue(key1.VerifyData("024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d".HexToBytes(), sig));
        }

        [TestMethod]
        public void TestSignRequest()
        {
            var key = "Kwk6k2eC3L3QuPvD8aiaNyoSXgQ2YL1bwS5CP1oKoA9waeAze97s".LoadWif();
            var req = new BalanceResponse
            {
                Body = new BalanceResponse.Types.Body
                {
                    Balance = new Decimal
                    {
                        Value = 100
                    },
                }
            };
            req.MetaHeader = new ResponseMetaHeader()
            {
                Ttl = 1
            };
            key.SignResponse(req);

            Assert.IsTrue(req.VerifyResponse());
        }

        [TestMethod]
        public void TestSignRFC6979()
        {
            var key = "L3o221BojgcCPYgdbXsm6jn7ayTZ72xwREvBHXKknR8VJ3G4WmjB".LoadWif();
            var body = "1f2155c0e513a7dab93d8b468809cd30a03c62326ec051deed031a6c6fbbdf02ca351745fa86b9ba5a9452d785ac4f7fc2b7548ca2a46c4fcf4a0000000053724e000000000000001e61".HexToBytes();
            var sig = key.SignRFC6979(body);
            Assert.AreEqual("e6e87810285bdcba714b5c7a38ca32a3759f3b6d10b3ae69a3e33a444dd7a0f5025a730e07a81170e89124a8115d10bf29446b4a09280e567fca4f27a6d0fcd8", sig.ToHexString());
        }

        [TestMethod]
        public void TestVerifyRFC6979()
        {
            var key = "L3o221BojgcCPYgdbXsm6jn7ayTZ72xwREvBHXKknR8VJ3G4WmjB".LoadWif();
            var body = "1f2155c0e513a7dab93d8b468809cd30a03c62326ec051deed031a6c6fbbdf02ca351745fa86b9ba5a9452d785ac4f7fc2b7548ca2a46c4fcf4a0000000053724e000000000000001e61".HexToBytes();
            var sig = key.SignRFC6979(body);
            Assert.IsTrue(key.PublicKey().VerifyRFC6979(body, sig));
        }

        [TestMethod]
        public void TestSignRFC6979_1()
        {
            var body = "1f2155c0e513a7dab93d8b468809cd30a03c62326ec051deed031a6c6fbbdf02ca351745fa86b9ba5a9452d785ac4f7fc2b7548ca2a46c4fcf4a0000000053724e000000000000001e61".HexToBytes();
            var wifs = new string[]
            {
                "L3o221BojgcCPYgdbXsm6jn7ayTZ72xwREvBHXKknR8VJ3G4WmjB",
                "KxLBJgapF8FucHptTm49BaW7wMmQGq97mdeYSnyDmVd39SCFxjBc",
                "Kyf6PExBaDHbYwmV7HAwaiqJM2x7wBqLHZrZqdKvF2RQsmDRfXgG",
                "KwMWwRYN72vFFV2pjTv9XrWz3tKCvANZQ5BHSKkGCBS5UdKeGTEG",
                "L4b9sjSbxfwJPUX9ESPV3LAU9PwLwrAuthVEpGc43Pm9ELtpky3b",
                "KyCQQ1LHNYsUQ2ZkD6yQry3hnMUWMvrbk9dHKBwq9ogGXippznKs",
            };
            var expect_sigs = new string[]
            {
                "e6e87810285bdcba714b5c7a38ca32a3759f3b6d10b3ae69a3e33a444dd7a0f5025a730e07a81170e89124a8115d10bf29446b4a09280e567fca4f27a6d0fcd8",
                "63efa21e48c3f56f9c8b1c7b24fa8141a4dd33cc12ae5d0bc2c885c628b2eb5b96aa1d7514e343c2221a49a8664415f1944452560bab13ff14ff3c6549da0665",
                "314e2f2adaaf67908aba6cb1183376574f27c1cafad5d8d852169f5adb26a6ac52641096d9051cdcd91e02420a0643827af535f25d8a9dafe4c5a00554a412ed",
                "9e155d31ba033d0637b03d9820c9312709376ab5d7f0a3b08392e47a5807bb76182f7add86b60723e120b25677626f3ab78fe04ed00ae0bb86503d8312c6d0bd",
                "f2de56e0777506fa52c19166706856794152a65e75a29dc6fc8a011d2abe1233c8e5e807a3d6b932e92ff91338755136446c57c9ec27df7eec04165e8ae38536",
                "9595d09ad84355e2c19ca5f08c19b9492816add735c7e01e549902f04b2c9d1ad7ea131a5e19ae325fffe5a4027383ff2132815c633abf318c2dc170890afc52",
            };
            for (int i = 0; i < wifs.Length; i++)
            {
                var key = wifs[i].LoadWif();
                var sig = key.SignRFC6979(body);
                Assert.AreEqual(expect_sigs[i], sig.ToHexString());
            }
        }
    }
}

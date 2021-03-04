using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Cryptography.Tz;

namespace Neo.FileSystem.API.UnitTests.TestCryptography.Tz
{
    [TestClass]
    public class UT_SL2
    {
        private SL2 Random()
        {
            var r = new SL2();
            r[0][0] = GF127.Random();
            r[0][1] = GF127.Random();
            r[1][0] = GF127.Random();
            // d = a^-1 * (1 + b*c)
            r[1][1] = GF127.Inv(r[0][0]) * (r[0][1] * r[1][0] + new GF127(1, 0));
            return r;
        }

        [TestMethod]
        public void TestInv()
        {
            for (int i = 0; i < 5; i++)
            {
                var a = Random();
                var b = SL2.Inv(a);
                var c = a * b;
                Assert.AreEqual(SL2.ID, c);
            }
        }

        [TestMethod]
        public void TestMulA()
        {
            var t = Random();
            var r1 = t.MulA();
            var r2 = t * SL2.A;
            Assert.AreEqual(r2, r1);
        }

        [TestMethod]
        public void TestMulB()
        {
            var t = Random();
            var r1 = t.MulB();
            var r2 = t * SL2.B;
            Assert.AreEqual(r2, r1);
        }
    }
}

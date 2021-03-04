using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Cryptography.Tz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neo.FileSystem.API.UnitTests.TestCryptography.Tz
{
    [TestClass]
    public class UT_GF127
    {
        private readonly GF127[][] MulTestCases = new GF127[][]
        {
            // (x+1)*(x^63+x^62+...+1) == x^64+1
            new []{new GF127(3, 0), new GF127(ulong.MaxValue, 0), new GF127(1, 1)},
            // x^126 * x^2 == x^128 == x^64 + x
            new []{new GF127(0, (ulong)1 << 62), new GF127(4, 0), new GF127(2, 1)},
            // (x^64+x^63+1) * (x^64+x) == x^128+x^65+x^127+x^64+x^64+x == x^65+x^64+x^63+1
            new []{new GF127(((ulong)1 << 63) + 1, 1), new GF127(2, 1), new GF127(0x8000000000000001, 3)},
        };

        private readonly GF127[][] Mul10TestCases = new GF127[][]
        {
            // 
            new []{new GF127(123, 0), new GF127(246, 0)},
            // 
            new []{new GF127(ulong.MaxValue, 2), new GF127(ulong.MaxValue - 1, 5)},
            // 
            new []{new GF127(0, ulong.MaxValue >> 1), new GF127(((ulong)1 << 63) + 1, (ulong.MaxValue >> 1) - 1)},
        };

        private readonly GF127[][] Mul11TestCases = new GF127[][]
        {
            // (x^6+x^5+x^4+x^3+x+1)(x+1) == x^7+x^3+x^2+1
            new []{new GF127(123, 0), new GF127(141, 0)},
            // 
            new []{new GF127(ulong.MaxValue, 2), new GF127(1, 7)},
            // 
            new []{new GF127(0, ulong.MaxValue >> 1), new GF127(((ulong)1 << 63) + 1, 1)},
        };

        private readonly GF127[][] InvTestCases = new GF127[][]
        {
            // 
            new []{new GF127(1, 0), new GF127(1, 0)},
            // 
            new []{new GF127(3, 0), new GF127(GF127.MSB64, ~GF127.MSB64)},
            // 
            new []{new GF127(54321, 12345), new GF127(8230555108620784737, 3929873967650665114)},
        };

        [TestMethod]
        public void Test1()
        {
            var a = ((ulong)1 << 63) + 1;
            Assert.AreEqual(0x8000000000000001, a);
        }

        [TestMethod]
        public void TestAdd()
        {
            var a = GF127.Random();
            var b = GF127.Random();
            var expected = new GF127(a[0] ^ b[0], a[1] ^ b[1]);
            var c = a + b;
            Assert.AreEqual(expected, c);
        }

        [TestMethod]
        public void TestMul()
        {
            foreach (var row in MulTestCases)
            {
                var c = row[0] * row[1];
                Assert.AreEqual(row[2], c);
            }
        }

        [TestMethod]
        public void TestMul10()
        {
            foreach (var row in Mul10TestCases)
            {
                var c = GF127.Mul10(row[0]);
                Assert.AreEqual(row[1], c);
            }
        }

        [TestMethod]
        public void TestMul11()
        {
            foreach (var row in Mul11TestCases)
            {
                var c = GF127.Mul11(row[0]);
                Assert.AreEqual(row[1], c);
            }
        }

        [TestMethod]
        public void TestInv()
        {
            foreach (var row in InvTestCases)
            {
                var c = GF127.Inv(row[0]);
                Assert.AreEqual(row[1], c);
            }

            for (int i = 0; i < 4; i++)
            {
                var a = GF127.Random();
                if (a.Equals(GF127.Zero))
                    continue;
                var b = GF127.Inv(a);
                var c = a * b;
                Assert.AreEqual(new GF127(1, 0), c);
            }
        }
    }
}

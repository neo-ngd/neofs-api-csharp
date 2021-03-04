using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Neo.FileSystem.API.Cryptography.Tz
{
    public class TzHash : HashAlgorithm
    {
        public const int TzHashLength = 64;
        private GF127[] x;
        public override int HashSize => TzHashLength;

        public override byte[] Hash => ToByteArray();


        public TzHash()
        {
            Initialize();
        }

        public override void Initialize()
        {
            this.x = new GF127[4];
            this.Reset();
        }

        public void Reset()
        {
            this.x[0] = new GF127(1, 0);
            this.x[1] = new GF127(0, 0);
            this.x[2] = new GF127(0, 0);
            this.x[3] = new GF127(1, 0);
        }

        public byte[] ToByteArray()
        {
            var buff = new byte[HashSize];
            for (int i = 0; i < 4; i++)
            {
                Array.Copy(this.x[i].ToByteArray(), 0, buff, i * 16, 16);
            }
            return buff;
        }

        [SecurityCritical]
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            _ = _HashData(array[ibStart..(ibStart + cbSize)]);
        }

        [SecurityCritical]
        protected override byte[] HashFinal()
        {
            return Hash;
        }

        [SecurityCritical]
        private int _HashData(byte[] data)
        {
            var n = data.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = 7; j >= 0; j--)
                {
                    MulBitRight(ref x[0], ref x[1], ref x[2], ref x[3], (data[i] & (1 << j)) != 0);
                }
            }
            return n;
        }

        // do not use this method, it's for fun, still in development
        public int HashDataParallel(byte[] data)
        {
            var n = data.Length;
            SL2[] t = new SL2[n * 8]; // caution out of memory exception

            Parallel.For(0, n, (i) =>
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((data[i] & (1 << (7 - j))) == 0)
                        t[i * 8 + j] = SL2.A;
                    else
                        t[i * 8 + j] = SL2.B;
                }
            });

            SL2[] r = t;
            do
            {
                r = MulParallel(r);
            } while (r.Length > 1);

            var r0 = r[0];
            x[0] = r0[0][0];
            x[1] = r0[0][1];
            x[2] = r0[1][0];
            x[3] = r0[1][1];
            return n;
        }

        private SL2[] MulParallel(SL2[] data)
        {
            var len = data.Length;
            //if (len == 1) return data;

            SL2[] t = new SL2[len / 2];

            if (len % 2 != 0)
            {
                data = data.Append(new SL2()).ToArray();
                t = t.Append(new SL2()).ToArray();
            }

            Parallel.For(0, t.Length, (i) =>
            {
                t[i] = data[i * 2] * data[i * 2 + 1];
            });

            return t;
            //return MulParallel(t);
        }

        // MulBitRight() multiply A (if the bit is 0) or B (if the bit is 1) on the right side 
        private void MulBitRight(ref GF127 c00, ref GF127 c01, ref GF127 c10, ref GF127 c11, bool bit)
        {
            // plan 1
            GF127 t;
            if (bit)
            {   // MulB
                t = c00;
                c00 = GF127.Mul10(c00) + c01; // c00 = c00 * x + c01
                c01 = GF127.Mul11(t) + c01;   // c01 = c00 * (x+1) + c01

                t = c10;
                c10 = GF127.Mul10(c10) + c11; // c10 = c10 * x + c11
                c11 = GF127.Mul11(t) + c11;   // c11 = c10 * (x+1) + c11
            }
            else
            {   // MulA
                t = c00;
                c00 = GF127.Mul10(c00) + c01; // c00 = c00 * x + c01
                c01 = t;                      // c01 = c00

                t = c10;
                c10 = GF127.Mul10(c10) + c11; // c10 = c10 * x + c11
                c11 = t;                      // c11 = c10;
            }

            //// plan 2
            //var r = new SL2(c00, c01, c10, c11);
            //if (bit)
            //    r.MulB();
            //else
            //    r.MulA();
        }

        // Concat() performs combining of hashes based on homomorphic characteristic.
        public static byte[] Concat(List<byte[]> hs)
        {
            var r = SL2.ID;
            foreach (var h in hs)
            {
                r *= new SL2().FromByteArray(h);
            }
            return r.ToByteArray();
        }

        // Validate() checks if hashes in hs combined are equal to h.
        public static bool Validate(byte[] h, List<byte[]> hs)
        {
            var expected = new SL2().FromByteArray(h);
            var actual = new SL2().FromByteArray(Concat(hs));
            return expected.Equals(actual);
        }

        // SubtractR() returns hash a, such that Concat(a, b) == c
        public static byte[] SubstractR(byte[] b, byte[] c)
        {
            var t1 = new SL2().FromByteArray(b);
            var t2 = new SL2().FromByteArray(c);
            var r = t2 * SL2.Inv(t1);
            return r.ToByteArray();
        }

        // SubtractL() returns hash b, such that Concat(a, b) == c
        public static byte[] SubstractL(byte[] a, byte[] c)
        {
            var t1 = new SL2().FromByteArray(a);
            var t2 = new SL2().FromByteArray(c);
            var r = SL2.Inv(t1) * t2;
            return r.ToByteArray();
        }
    }
}

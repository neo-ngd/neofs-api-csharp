using System;
using System.Security.Cryptography;

namespace Neo.FileSystem.API.Cryptography.Tz
{
    // GF127 represents element of GF(2^127)
    public class GF127 : IEquatable<GF127>
    {
        public const int ByteSize = 16;
        public const ulong MaxUlong = ulong.MaxValue;
        public const ulong MSB64 = (ulong)1 << 63; // 2^63
        public static readonly GF127 Zero = new GF127(0, 0);
        public static readonly GF127 One = new GF127(1, 0);
        public static readonly GF127 X127X631 = new GF127(MSB64 + 1, MSB64); // x^127+x^63+1

        private readonly ulong[] _data;

        public ulong this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public GF127(ulong[] value)
        {
            if (value is null || value.Length != 2)
                throw new ArgumentException();
            _data = value;
        }

        // Constructs new element of GF(2^127) as u1*x^64 + u0.
        // It is assumed that u1 has zero MSB.
        public GF127(ulong u0, ulong u1) : this(new ulong[] { u0, u1 })
        {
        }

        public GF127() : this(0, 0)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (!(obj is GF127 b))
                return false;
            return Equals(b);
        }

        public override int GetHashCode()
        {
            return this[0].GetHashCode() + this[1].GetHashCode();
        }

        public bool Equals(GF127 other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this[0] == other[0] && this[1] == other[1];
        }

        // return the index of MSB
        private int IndexOfMSB()
        {
            int i = Helper.GetLeadingZeros(this[1]);
            if (i == 64)
                i += Helper.GetLeadingZeros(this[0]);
            return 127 - i;
        }

        // Set index n to 1
        public static GF127 SetN(int n)
        {
            if (n < 64)
                return new GF127((ulong)1 << n, 0);
            return new GF127(0, (ulong)1 << (n - 64));
        }

        // Add
        public static GF127 operator +(GF127 a, GF127 b)
        {
            return new GF127(a[0] ^ b[0], a[1] ^ b[1]);
        }

        // Bitwise-and
        public static GF127 operator &(GF127 a, GF127 b)
        {
            return new GF127(a[0] & b[0], a[1] & b[1]);
        }

        // Multiply
        public static GF127 operator *(GF127 a, GF127 b) // 2^63 * 2,  10
        {
            GF127 r = new GF127();
            GF127 c = a;

            if (b[1] == 0)
            {
                for (int i = 0; i < b[0].GetNonZeroLength(); i++)
                {
                    if ((b[0] & ((ulong)1 << i)) != 0)
                        r += c;
                    c = Mul10(c);                       // c = c * 2
                }
            }
            else
            {
                for (int i = 0; i < 64; i++)
                {
                    if ((b[0] & ((ulong)1 << i)) != 0)
                        r += c;
                    c = Mul10(c);                       // c = c * 2
                }
                for (int i = 0; i < b[1].GetNonZeroLength(); i++)
                {
                    if ((b[1] & ((ulong)1 << i)) != 0)
                        r += c;
                    c = Mul10(c);
                }
            }
            return r;
        }

        // Inverse, returns a^-1
        // Extended Euclidean Algorithm
        // https://link.springer.com/content/pdf/10.1007/3-540-44499-8_1.pdf
        public static GF127 Inv(GF127 a)
        {
            GF127 v = X127X631,
                u = a,
                c = new GF127(1, 0),
                d = new GF127(0, 0),
                t,
                x;

            int du = u.IndexOfMSB();
            int dv = v.IndexOfMSB();
            // degree of polynomial is a position of most significant bit
            while (du != 0)
            {
                if (du < dv)
                {
                    (v, u) = (u, v);
                    (dv, du) = (du, dv);
                    (d, c) = (c, d);
                }
                x = SetN(du - dv);
                t = x * v;
                u += t;
                // because * performs reduction on t, manually reduce u at first step
                if (u.IndexOfMSB() == 127)
                    u += X127X631;

                t = x * d;
                c += t;

                du = u.IndexOfMSB();
                dv = v.IndexOfMSB();
            }
            return c;
        }

        // Mul10 returns a*x
        public static GF127 Mul10(GF127 a)
        {
            GF127 b = new GF127();
            var c = (a[0] & MSB64) >> 63;
            b[0] = a[0] << 1;
            b[1] = (a[1] << 1) ^ c;
            if ((b[1] & MSB64) != 0)
            {
                b[0] ^= X127X631[0];
                b[1] ^= X127X631[1];
            }
            return b;
        }

        // Mul11 returns a*(x+1)
        public static GF127 Mul11(GF127 a)
        {
            GF127 b = new GF127();
            var c = (a[0] & MSB64) >> 63;
            b[0] = a[0] ^ (a[0] << 1);
            b[1] = a[1] ^ (a[1] << 1) ^ c;
            if ((b[1] & MSB64) != 0)
            {
                b[0] ^= X127X631[0];
                b[1] ^= X127X631[1];
            }
            return b;
        }

        // Random returns random element from GF(2^127).
        // Is used mostly for testing.
        public static GF127 Random()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            return new GF127(rng.NextUlong(), rng.NextUlong() >> 1);
        }

        // FromByteArray does the deserialization stuff
        public GF127 FromByteArray(byte[] data)
        {
            if (data.Length != ByteSize)
                throw new ArgumentException();
            var t0 = new byte[8];
            var t1 = new byte[8];
            Array.Copy(data, 0, t1, 0, 8);
            Array.Copy(data, 8, t0, 0, 8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(t0);
                Array.Reverse(t1);
            }
            _data[0] = BitConverter.ToUInt64(t0);
            _data[1] = BitConverter.ToUInt64(t1);
            if ((_data[1] & MSB64) != 0)
                throw new ArgumentException();
            return this;
        }

        // ToArray() represents element of GF(2^127) as byte array of length 16.
        public byte[] ToByteArray()
        {
            var buff = new byte[16];
            var b0 = BitConverter.GetBytes(_data[0]);
            var b1 = BitConverter.GetBytes(_data[1]);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(b0);
                Array.Reverse(b1);
            }
            Array.Copy(b1, 0, buff, 0, 8);
            Array.Copy(b0, 0, buff, 8, 8);
            return buff;
        }

        // ToString() returns hex-encoded representation, starting with MSB.
        public override string ToString()
        {
            return this.ToByteArray().ToHexString();
        }

    }
}

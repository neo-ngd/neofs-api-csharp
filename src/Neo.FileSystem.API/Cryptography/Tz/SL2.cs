using System;
using System.Linq;

namespace Neo.FileSystem.API.Cryptography.Tz
{
    public class SL2 : IEquatable<SL2>
    {
        // 2x2 matrix
        private readonly GF127[][] data;

        public static readonly SL2 ID = new SL2(new GF127(1, 0), new GF127(0, 0),
                                                new GF127(0, 0), new GF127(1, 0));
        public static readonly SL2 A = new SL2(new GF127(2, 0), new GF127(1, 0),
                                                new GF127(1, 0), new GF127(0, 0));
        public static readonly SL2 B = new SL2(new GF127(2, 0), new GF127(3, 0),
                                                new GF127(1, 0), new GF127(1, 0));

        // Indexer
        public GF127[] this[int i]
        {
            get { return data[i]; }
            set { data[i] = value; }
        }

        public SL2(GF127[][] value)
        {
            if (value is null || value.Length != 2 || !value.All(p => p.Length == 2))
                throw new ArgumentException();
            data = value;
        }

        public SL2(GF127 g00, GF127 g01, GF127 g10, GF127 g11)
            : this(new GF127[][] { new[] { g00, g01 }, new[] { g10, g11 } })
        {
        }

        public SL2() : this(GF127.One, GF127.Zero, GF127.Zero, GF127.One)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (!(obj is SL2 b))
                return false;
            return Equals(b);
        }

        public override int GetHashCode()
        {
            return this[0][0].GetHashCode() +
                   this[0][1].GetHashCode() +
                   this[1][0].GetHashCode() +
                   this[1][1].GetHashCode();
        }

        public bool Equals(SL2 other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this[0][0].Equals(other[0][0]) &&
                   this[0][1].Equals(other[0][1]) &&
                   this[1][0].Equals(other[1][0]) &&
                   this[1][1].Equals(other[1][1]);
        }


        // 2X2 matrix multiplication
        public static SL2 operator *(SL2 a, SL2 b)
        {
            return new SL2(a[0][0] * b[0][0] + a[0][1] * b[1][0], a[0][0] * b[0][1] + a[0][1] * b[1][1],
                           a[1][0] * b[0][0] + a[1][1] * b[1][0], a[1][0] * b[0][1] + a[1][1] * b[1][1]);
        }

        // Multiplication using strassen algorithm
        public static SL2 MulStrassen(SL2 a, SL2 b)
        {
            GF127[] t = new GF127[7];
            t[0] = (a[0][0] + a[1][1]) * (b[0][0] + b[1][1]); // t[0] == (a11 + a22) * (b11 + b22)

            t[1] = (a[1][0] + a[1][1]) * b[0][0]; // t[1] == (a21 + a22) * b11

            t[2] = (b[0][1] + b[1][1]) * a[0][0]; // t[2] == (b12 + b22) * a11

            t[3] = (b[1][0] + b[0][0]) * a[1][1]; // t[3] == (b21 + b11) * a22

            t[4] = (a[0][0] + a[0][1]) * b[1][1]; // t[4] == (a11 + a12) * b22

            t[5] = (a[1][0] + a[0][0]) * (b[0][0] + b[0][1]); // t[5] == (a21 + a11) * (b11 + b12)

            t[6] = (a[0][1] + a[1][1]) * (b[1][0] + b[1][1]); // t[6] == (a12 + a22) * (b21 + b22)

            SL2 r = new SL2();
            r[0][1] = t[2] + t[4]; // r12 == a11*b12 + a11*b22 + a11*b22 + a12*b22 == a11*b12 + a12*b22
            r[1][0] = t[1] + t[3]; // r21 == a21*b11 + a22*b11 + a22*b21 + a22*b11 == a21*b11 + a22*b21
            // r11 == (a11*b11 + a22*b11` + a11*b22` + a22*b22`) + (a22*b21` + a22*b11`) + (a11*b22` + a12*b22`) + (a12*b21 + a22*b21` + a12*b22` + a22*b22`)
            //     == a11*b11 + a12*b21
            r[0][0] = t[0] + t[3] + t[4] + t[6];
            // r22 == (a11*b11` + a22*b11` + a11*b22` + a22*b22) + (a21*b11` + a22*b11`) + (a11*b12` + a11*b22`) + (a21*b11` + a11*b11` + a21*b12 + a11*b12`)
            //     == a21*b12 + a22*b22
            r[1][1] = t[0] + t[1] + t[2] + t[5];

            return r;
        }

        // Inv() returns inverse of a in SL2(GF(2^127))
        public static SL2 Inv(SL2 a)
        {
            GF127[] t = new GF127[2];
            t[0] = a[0][0] * a[1][1] + a[0][1] * a[1][0]; // 
            t[1] = GF127.Inv(t[0]);

            SL2 r = new SL2();
            r[1][1] = t[1] * a[0][0];
            r[0][1] = t[1] * a[0][1];
            r[1][0] = t[1] * a[1][0];
            r[0][0] = t[1] * a[1][1];

            return r;
        }

        // MulA() returns this*A, A = {{x, 1}, {1, 0}}
        public SL2 MulA()
        {
            var r = new SL2();
            r[0][0] = GF127.Mul10(this[0][0]) + this[0][1]; // r11 == t11*x + t12
            r[0][1] = this[0][0];                           // r12 == t11

            r[1][0] = GF127.Mul10(this[1][0]) + this[1][1]; // r21 == t21*x + t22
            r[1][1] = this[1][0];                           // r22 == t21

            return r;
        }

        // MulB() returns this*B, B = {{x, x+1}, {1, 1}}
        public SL2 MulB()
        {
            var r = new SL2();
            r[0][0] = GF127.Mul10(this[0][0]) + this[0][1];              // r11 == t11*x + t12
            r[0][1] = GF127.Mul10(this[0][0]) + this[0][0] + this[0][1]; // r12 == t11*x + t11 + t12

            r[1][0] = GF127.Mul10(this[1][0]) + this[1][1];              // r21 == t21*x + t22
            r[1][1] = GF127.Mul10(this[1][0]) + this[1][0] + this[1][1]; // r22 == t21*x + t21 + t22

            return r;
        }

        public SL2 FromByteArray(byte[] data)
        {
            if (data.Length != 64)
                throw new ArgumentException();
            this[0][0] = new GF127().FromByteArray(data[0..16]);
            this[0][1] = new GF127().FromByteArray(data[16..32]);
            this[1][0] = new GF127().FromByteArray(data[32..48]);
            this[1][1] = new GF127().FromByteArray(data[48..64]);
            return this;
        }

        public byte[] ToByteArray()
        {
            var buff = new byte[64];
            Array.Copy(this[0][0].ToByteArray(), 0, buff, 0, 16);
            Array.Copy(this[0][1].ToByteArray(), 0, buff, 16, 16);
            Array.Copy(this[1][0].ToByteArray(), 0, buff, 32, 16);
            Array.Copy(this[1][1].ToByteArray(), 0, buff, 48, 16);
            return buff;
        }

        public override string ToString()
        {
            return this[0][0].ToString() + this[0][1].ToString() +
                   this[1][0].ToString() + this[1][1].ToString();
        }
    }
}

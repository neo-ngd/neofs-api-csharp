using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Neo.FileSystem.API.Cryptography
{
    internal class Murmur3_128 : HashAlgorithm
    {
        private const ulong c1 = 0x87c37b91114253d5;
        private const ulong c2 = 0x4cf5ad432745937f;
        private const uint m = 5;
        private const uint n1 = 0x52dce729;
        private const uint n2 = 0x38495ab5;

        private ulong h1;
        private ulong h2;
        private readonly uint seed;
        private int length;

        public Murmur3_128(uint seed)
        {
            this.seed = seed;
            Initialize();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            length += cbSize;
            int remainder = cbSize & 15;
            int alignedLength = ibStart + (cbSize - remainder);
            for (int i = ibStart; i < alignedLength; i += 16)
            {
                ulong k1 = BinaryPrimitives.ReadUInt64LittleEndian(array.AsSpan(i));
                ulong k2 = BinaryPrimitives.ReadUInt64LittleEndian(array.AsSpan(i + 8));

                k1 *= c1;
                k1 = (k1 << 31) | (k1 >> 33);
                k1 *= c2;
                h1 ^= k1;
                h1 = (h1 << 27) | (h1 >> 37);
                h1 += h2;
                h1 = h1 * m + n1;

                k2 *= c2;
                k2 = (k2 << 33) | (k2 >> 31);
                k2 *= c1;
                h2 ^= k2;
                h2 = (h2 << 31) | (h2 >> 33);
                h2 += h1;
                h2 = h2 * m + n2;
            }
            if (remainder > 0)
            {
                ulong k1 = 0, k2 = 0;
                switch (remainder)
                {
                    case 15: k2 ^= (ulong)array[alignedLength + 14] << 48; goto case 14;
                    case 14: k2 ^= (ulong)array[alignedLength + 13] << 40; goto case 13;
                    case 13: k2 ^= (ulong)array[alignedLength + 12] << 32; goto case 12;
                    case 12: k2 ^= (ulong)array[alignedLength + 11] << 24; goto case 11;
                    case 11: k2 ^= (ulong)array[alignedLength + 10] << 16; goto case 10;
                    case 10: k2 ^= (ulong)array[alignedLength + 9] << 8; goto case 9;
                    case 9:
                        {
                            k2 ^= (ulong)array[alignedLength + 8];
                            k2 *= c2;
                            k2 = (k2 << 33) | (k2 >> 31);
                            k2 *= c1;
                            h2 ^= k2;
                            goto case 8;
                        }
                    case 8: k1 ^= (ulong)array[alignedLength + 7] << 56; goto case 7;
                    case 7: k1 ^= (ulong)array[alignedLength + 6] << 48; goto case 6;
                    case 6: k1 ^= (ulong)array[alignedLength + 5] << 40; goto case 5;
                    case 5: k1 ^= (ulong)array[alignedLength + 4] << 32; goto case 4;
                    case 4: k1 ^= (ulong)array[alignedLength + 3] << 24; goto case 3;
                    case 3: k1 ^= (ulong)array[alignedLength + 2] << 16; goto case 2;
                    case 2: k1 ^= (ulong)array[alignedLength + 1] << 8; goto case 1;
                    case 1:
                        {
                            k1 ^= (ulong)array[alignedLength];
                            k1 *= c1;
                            k1 = (k1 << 31) | (k1 >> 33);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                        }
                }
            }
        }

        protected override byte[] HashFinal()
        {
            h1 ^= (ulong)length;
            h2 ^= (ulong)length;
            h1 += h2;
            h2 += h1;
            h1 = fimix64(h1);
            h2 = fimix64(h2);
            h1 += h2;
            h2 += h1;
            return BitConverter.GetBytes(h1);
        }

        public override void Initialize()
        {
            length = 0;
            h1 = seed;
            h2 = seed;
        }

        private ulong fimix64(ulong k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccd;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53;
            k ^= k >> 33;
            return k;
        }
    }
}

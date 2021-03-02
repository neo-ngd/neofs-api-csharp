using Google.Protobuf;
using NeoFS.API.v2.Refs;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace NeoFS.API.v2.Cryptography
{
    public static class Crypto
    {
        public static byte[] RIPEMD160(this byte[] value)
        {
            var hash = new byte[20];
            var digest = new RipeMD160Digest();
            digest.BlockUpdate(value, 0, value.Length);
            digest.DoFinal(hash, 0);
            return hash;
        }

        public static byte[] Sha256(this byte[] value)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(value);
        }

        public static byte[] Sha256(this byte[] value, int offset, int count)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(value, offset, count);
        }

        public static byte[] Sha256(this ReadOnlySpan<byte> value)
        {
            byte[] buffer = new byte[32];
            using var sha256 = SHA256.Create();
            sha256.TryComputeHash(value, buffer, out _);
            return buffer;
        }
        public static ByteString Sha256(this IMessage data)
        {
            return ByteString.CopyFrom(data.ToByteArray().Sha256());
        }

        public static ByteString Sha256(this ByteString data)
        {
            return ByteString.CopyFrom(data.ToByteArray().Sha256());
        }

        public static Checksum Sha256Checksum(this IMessage data)
        {
            return new Checksum
            {
                Type = ChecksumType.Sha256,
                Sum = data.Sha256()
            };
        }

        public static Checksum Sha256Checksum(this ByteString data)
        {
            return new Checksum
            {
                Type = ChecksumType.Sha256,
                Sum = data.Sha256()
            };
        }

        public static ulong Murmur64(this byte[] value, uint seed)
        {
            using Murmur3_128 murmur = new Murmur3_128(seed);
            return BinaryPrimitives.ReadUInt64LittleEndian(murmur.ComputeHash(value));
        }
    }
}
using Google.Protobuf;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Cryptography.Tz;
using System;

namespace Neo.FileSystem.API.Refs
{
    public sealed partial class Checksum
    {
        public Checksum(byte[] hash)
        {
            if (hash.Length == Crypto.Sha256HashLength)
            {
                type_ = ChecksumType.Sha256;
                sum_ = ByteString.CopyFrom(hash);
            }
            else if (hash.Length == TzHash.TzHashLength)
            {
                type_ = ChecksumType.Tz;
                sum_ = ByteString.CopyFrom(hash);
            }
            else
            {
                throw new InvalidOperationException(nameof(Checksum) + " unsupported hash length");
            }

        }

        public bool Verify(ByteString data)
        {
            switch (type_)
            {
                case ChecksumType.Sha256:
                    {
                        return sum_ == data.Sha256();
                    }
                case ChecksumType.Tz:
                    {
                        //TODO
                        throw new NotImplementedException();
                    }
                default:
                    throw new InvalidOperationException(nameof(Verify) + " unsupported checksum type " + type_);
            }
        }

        public string String()
        {
            return sum_.ToByteArray().ToHexString();
        }

        public void Parse(string str)
        {
            sum_ = ByteString.CopyFrom(str.HexToBytes());
            switch (sum_.Length)
            {
                case Crypto.Sha256HashLength:
                    type_ = ChecksumType.Sha256;
                    break;
                case TzHash.TzHashLength:
                    type_ = ChecksumType.Tz;
                    break;
                default:
                    throw new FormatException($"unsupported checksum length {sum_.Length}");
            }
        }
    }
}
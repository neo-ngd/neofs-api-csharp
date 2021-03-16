using Google.Protobuf;
using Neo.FileStorage.API.Cryptography.Tz;
using Neo.IO.Json;
using System;
using static Neo.FileStorage.API.Cryptography.Helper;

namespace Neo.FileStorage.API.Refs
{
    public sealed partial class Checksum
    {
        public Checksum(byte[] hash)
        {
            if (hash.Length == Sha256HashLength)
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
                        return sum_ == data.TzHash();
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
                case Sha256HashLength:
                    type_ = ChecksumType.Sha256;
                    break;
                case TzHash.TzHashLength:
                    type_ = ChecksumType.Tz;
                    break;
                default:
                    throw new FormatException($"unsupported checksum length {sum_.Length}");
            }
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["type"] = Type.ToString();
            json["sum"] = Sum.ToBase64();
            return json;
        }
    }
}
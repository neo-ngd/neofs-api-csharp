using Google.Protobuf;
using Neo.FileSystem.API.Cryptography;

namespace Neo.FileSystem.API.Refs
{
    public partial class OwnerID
    {
        public const int ValueSize = 25;

        public static OwnerID Frombytes(byte[] bytes)
        {
            if (bytes.Length != ValueSize) throw new System.InvalidOperationException("OwnerID must be a hash256");
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes)
            };
        }

        public static OwnerID FromBase58String(string id)
        {
            return Frombytes(Base58.Decode(id));
        }

        public string ToBase58String()
        {
            return Base58.Encode(Value.ToByteArray());
        }
    }
}

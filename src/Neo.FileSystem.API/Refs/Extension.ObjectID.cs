using Google.Protobuf;
using Neo.FileSystem.API.Cryptography;

namespace Neo.FileSystem.API.Refs
{
    public partial class ObjectID
    {
        //Hash256 to ObjectID
        public static ObjectID FromSha256Bytes(byte[] hash)
        {
            if (hash.Length != Crypto.Sha256HashLength) throw new System.InvalidOperationException("ObjectID must be a hash256");
            return new ObjectID
            {
                Value = ByteString.CopyFrom(hash)
            };
        }

        public static ObjectID FromBase58String(string id)
        {
            return FromSha256Bytes(Base58.Decode(id));
        }

        public string ToBase58String()
        {
            return Base58.Encode(Value.ToByteArray());
        }
    }
}

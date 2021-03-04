using Google.Protobuf;
using System;

namespace Neo.FileSystem.API.Object
{
    public class SplitID : IComparable<SplitID>, IEquatable<SplitID>
    {
        private Guid guid;

        public SplitID() { }

        public SplitID(byte[] bytes)
        {
            guid = new Guid(bytes);
        }

        public SplitID(ByteString bs)
        {
            guid = new Guid(bs.ToByteArray());
        }

        public static SplitID FromBytes(byte[] bytes)
        {
            var sid = new SplitID();
            sid.guid = new Guid(bytes);
            return sid;
        }

        public static SplitID FromByteString(ByteString bstr)
        {
            if (bstr != null)
                return FromBytes(bstr.ToByteArray());
            return null;
        }

        public bool Parse(string str)
        {
            return Guid.TryParse(str, out guid);
        }

        public override string ToString()
        {
            return guid == Guid.Empty ? "" : guid.ToString();
        }

        public void SetGuid(Guid g)
        {
            if (g != Guid.Empty)
                guid = g;
        }

        public byte[] ToBytes()
        {
            return guid == Guid.Empty ? Array.Empty<byte>() : guid.ToByteArray();
        }

        public ByteString ToByteString()
        {
            return ByteString.CopyFrom(ToBytes());
        }

        public bool Equals(SplitID other)
        {
            if (guid == Guid.Empty || other.guid == Guid.Empty)
                return false;
            return ToString() == other.ToString();
        }

        public int CompareTo(SplitID other)
        {
            return ToString().CompareTo(other.ToString());
        }
    }
}

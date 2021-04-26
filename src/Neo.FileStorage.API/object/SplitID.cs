using Google.Protobuf;
using System;

namespace Neo.FileStorage.API.Object
{
    public class SplitID : IComparable<SplitID>, IEquatable<SplitID>
    {
        public const int Size = 16;

        private Guid guid;

        public SplitID()
        {
            guid = Guid.NewGuid();
        }

        public SplitID(byte[] bytes)
        {
            guid = new Guid(bytes);
        }

        public SplitID(ByteString bs)
        {
            guid = new Guid(bs.ToByteArray());
        }

        public static SplitID FromByteArray(byte[] bytes)
        {
            SplitID sid = new(bytes);
            return sid;
        }

        public static SplitID FromByteString(ByteString bstr)
        {
            if (bstr != null)
                return FromByteArray(bstr.ToByteArray());
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

        public byte[] ToByteArray()
        {
            return guid == Guid.Empty ? Array.Empty<byte>() : guid.ToByteArray();
        }

        public ByteString ToByteString()
        {
            return ByteString.CopyFrom(ToByteArray());
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

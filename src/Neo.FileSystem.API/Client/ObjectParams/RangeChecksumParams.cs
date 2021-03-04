using Neo.FileSystem.API.Object;
using Neo.FileSystem.API.Refs;
using System.Collections.Generic;

namespace Neo.FileSystem.API.Client.ObjectParams
{
    public class RangeChecksumParams
    {
        public Address Address;
        public bool Raw;
        public List<Range> Ranges;
        public ChecksumType Type;
        public byte[] Salt;
    }
}

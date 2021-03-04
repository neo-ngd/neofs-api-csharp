namespace Neo.FileSystem.API.Session
{
    public partial class XHeader
    {
        public const string ReservedXHeaderPrefix = "__NEOFS__";
        public const string XHeaderNetmapEpoch = ReservedXHeaderPrefix + "NETMAP_EPOCH";
        public const string XHeaderNetmapLookupDepth = ReservedXHeaderPrefix + "NETMAP_LOOKUP_DEPTH";
    }
}

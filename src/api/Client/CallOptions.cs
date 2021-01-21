using NeoFS.API.v2.Acl;
using NeoFS.API.v2.Session;
using NeoFS.API.v2.Refs;

namespace NeoFS.API.v2.Client
{
    public class CallOptions
    {
        public Version Version;
        public uint Ttl;
        public ulong Epoch;
        public XHeader[] XHeaders;
        public SessionToken Session;
        public BearerToken Bearer;

        public RequestMetaHeader GetRequestMetaHeader()
        {
            var meta = new RequestMetaHeader
            {
                Version = Version,
                Ttl = Ttl,
                Epoch = Epoch,
            };
            if (XHeaders != null) meta.XHeaders.AddRange(XHeaders);
            if (Session != null) meta.SessionToken = Session;
            if (Bearer != null) meta.BearerToken = Bearer;
            return meta;
        }

        public CallOptions ApplyCustomOptions(CallOptions custom)
        {
            if (custom is null) return this;
            if (custom.Version != null) Version = custom.Version;
            Ttl = custom.Ttl;
            Epoch = custom.Epoch;
            if (custom.XHeaders != null) XHeaders = custom.XHeaders;
            if (custom.Session != null) Session = custom.Session;
            if (custom.Bearer != null) Bearer = custom.Bearer;
            return this;
        }
    }
}

using Neo.FileSystem.API.Refs;

namespace Neo.FileSystem.API.Session
{
    public partial class RequestMetaHeader
    {
        public static RequestMetaHeader Default
        {
            get
            {
                var meta = new RequestMetaHeader()
                {
                    Version = Version.SDKVersion(),
                    Epoch = 0,
                    Ttl = 2,
                };
                return meta;
            }
        }
    }
}
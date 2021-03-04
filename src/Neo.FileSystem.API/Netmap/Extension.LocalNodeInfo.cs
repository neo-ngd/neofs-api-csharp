using Google.Protobuf;
using Neo.FileSystem.API.Session;

namespace Neo.FileSystem.API.Netmap
{
    public partial class LocalNodeInfoRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class LocalNodeInfoResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }
}

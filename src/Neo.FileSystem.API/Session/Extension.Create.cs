using Google.Protobuf;

namespace Neo.FileSystem.API.Session
{
    public partial class CreateResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class CreateRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }
}

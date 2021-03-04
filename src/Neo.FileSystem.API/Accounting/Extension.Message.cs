using Google.Protobuf;
using Neo.FileSystem.API.Session;

namespace Neo.FileSystem.API.Accounting
{
    public partial class BalanceRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class BalanceResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }
}

using Neo.FileSystem.API.Session;
using Google.Protobuf;

namespace Neo.FileSystem.API.Session
{
    public interface IRequestMeta
    {
        RequestMetaHeader MetaHeader { get; set; }
    }

    public interface IRequest : IRequestMeta
    {
        RequestVerificationHeader VerifyHeader { get; set; }
        IMessage GetBody();
    }
}
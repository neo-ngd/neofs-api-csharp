using Neo.FileSystem.API.Session;
using Google.Protobuf;

namespace Neo.FileSystem.API.Session
{
    public interface IResponseMeta
    {
        ResponseMetaHeader MetaHeader { get; set; }
    }

    public interface IResponse : IResponseMeta
    {
        ResponseVerificationHeader VerifyHeader { get; set; }
        IMessage GetBody();
    }
}
﻿using Google.Protobuf;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Accounting
{
    public partial class BalanceRequest : IRequest
    {
        IMetaHeader IVerificableMessage.GetMetaHeader()
        {
            return MetaHeader;
        }

        IVerificationHeader IVerificableMessage.GetVerificationHeader()
        {
            return VerifyHeader;
        }

        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class BalanceResponse : IResponse
    {
        IMetaHeader IVerificableMessage.GetMetaHeader()
        {
            return MetaHeader;
        }

        IVerificationHeader IVerificableMessage.GetVerificationHeader()
        {
            return VerifyHeader;
        }

        public IMessage GetBody()
        {
            return Body;
        }
    }
}

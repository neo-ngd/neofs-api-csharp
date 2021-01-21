using NeoFS.API.v2.Acl;
using NeoFS.API.v2.Cryptography;
using NeoFS.API.v2.Session;
using System;
using System.Threading;

namespace NeoFS.API.v2.Client
{
    public partial class Client
    {
        public void AttachSessionToken(SessionToken token)
        {
            session = token;
        }

        public void AttachBearerToken(BearerToken token)
        {
            bearer = token;
        }

        public SessionToken CreateSession(CancellationToken context, ulong expiration, CallOptions option = null)
        {
            var session_client = new SessionService.SessionServiceClient(channel);
            var req = new CreateRequest
            {
                Body = new CreateRequest.Types.Body
                {
                    OwnerId = key.ToOwnerID(),
                    Expiration = expiration,
                }
            };
            req.MetaHeader = option?.GetRequestMetaHeader() ?? RequestMetaHeader.Default;
            req.SignRequest(key);

            var resp = session_client.Create(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new FormatException("invalid balance response");
            return new SessionToken
            {
                Body = new SessionToken.Types.Body
                {
                    Id = resp.Body.Id,
                    SessionKey = resp.Body.SessionKey,
                    OwnerId = key.ToOwnerID(),
                }
            };
        }
    }
}

using Neo.FileSystem.API.Acl;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Session;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.FileSystem.API.Client
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

        public async Task<SessionToken> CreateSession(CancellationToken context, ulong expiration, CallOptions option = null)
        {
            var session_client = new SessionService.SessionServiceClient(channel);
            var req = new CreateRequest
            {
                Body = new CreateRequest.Types.Body
                {
                    OwnerId = key.ToOwnerID_obsolete(),
                    Expiration = expiration,
                }
            };
            req.MetaHeader = option?.GetRequestMetaHeader() ?? RequestMetaHeader.Default;
            key.SignRequest(req);

            var resp = await session_client.CreateAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new FormatException("invalid balance response");
            return new SessionToken
            {
                Body = new SessionToken.Types.Body
                {
                    Id = resp.Body.Id,
                    SessionKey = resp.Body.SessionKey,
                    OwnerId = key.ToOwnerID_obsolete(),
                }
            };
        }
    }
}

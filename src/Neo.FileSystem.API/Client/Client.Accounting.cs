using Neo.FileSystem.API.Accounting;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Refs;
using System;
using System.Threading.Tasks;

namespace Neo.FileSystem.API.Client
{
    public partial class Client
    {
        public async Task<Accounting.Decimal> GetBalance(OwnerID owner, CallOptions options = null)
        {
            var account_client = new AccountingService.AccountingServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new BalanceRequest
            {
                Body = new BalanceRequest.Types.Body
                {
                    OwnerId = owner,
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);
            var resp = await account_client.BalanceAsync(req);
            if (!resp.VerifyResponse())
                throw new FormatException("invalid balance response");
            return resp.Body.Balance;
        }

        public async Task<Accounting.Decimal> GetSelfBalance()
        {
            var w = key.ToOwnerID_obsolete();
            return await GetBalance(w);
        }
    }
}

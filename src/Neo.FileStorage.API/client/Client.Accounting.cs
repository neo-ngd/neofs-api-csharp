using Neo.FileStorage.API.Accounting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using System;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<Accounting.Decimal> GetBalance(OwnerID owner, CallOptions options = null)
        {
            if (owner is null) throw new ArgumentNullException(nameof(owner));
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
            var w = key.ToOwnerID();
            return await GetBalance(w);
        }
    }
}

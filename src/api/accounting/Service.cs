﻿using System;
using System.Security.Cryptography;
using Google.Protobuf;
using Grpc.Core;
using NeoFS.API.v2;
using NeoFS.Crypto;

namespace NeoFS.API.v2.Accounting
{
    public static class AccountingExtension
    {
        public static BalanceResponse GetBalance(this Channel chan, uint ttl, ECDsa key, bool debug = false)
        {
            var req = new BalanceRequest
            {
                Body = new BalanceRequest.Types.Body
                {
                    OwnerId = new Refs.OwnerID
                    {
                        Value = ByteString.CopyFrom(key.Address()),
                    },
                },
            };

            // req.SetTTL(ttl);
            // req.SignHeader(key, debug);

            return new AccountingService.AccountingServiceClient(chan).Balance(req);
        }
    }
}

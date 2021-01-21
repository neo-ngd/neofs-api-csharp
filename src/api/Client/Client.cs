using Grpc.Core;
using NeoFS.API.v2.Acl;
using NeoFS.API.v2.Refs;
using NeoFS.API.v2.Session;
using System;
using System.Security.Cryptography;

namespace NeoFS.API.v2.Client
{
    public partial class Client
    {
        const uint SearchObjectVersion = 1;
        private readonly ECDsa key;
        private readonly Channel channel;
        private SessionToken session;
        private BearerToken bearer;

        public Client(string host, ECDsa k)
        {
            channel = new Channel(host, ChannelCredentials.Insecure);
            key = k;
        }

        public CallOptions DefaultCallOptions
        {
            get
            {
                return new CallOptions
                {
                    Version = Refs.Version.SDKVersion(),
                    Ttl = 2,
                    Session = session,
                    Bearer = bearer,
                };
            }
        }
    }
}

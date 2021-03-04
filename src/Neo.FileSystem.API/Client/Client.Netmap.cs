using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Netmap;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.FileSystem.API.Client
{
    public partial class Client
    {
        public async Task<NodeInfo> LocalNodeInfo(CancellationToken context, CallOptions options = null)
        {
            var netmap_client = new NetmapService.NetmapServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new LocalNodeInfoRequest
            {
                Body = new LocalNodeInfoRequest.Types.Body { }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);
            var resp = await netmap_client.LocalNodeInfoAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new FormatException(nameof(LocalNodeInfo) + " invalid LocalNodeInfo response");
            return resp.Body.NodeInfo;
        }

        public async Task<ulong> Epoch(CancellationToken context, CallOptions options = null)
        {
            var netmap_client = new NetmapService.NetmapServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new LocalNodeInfoRequest
            {
                Body = new LocalNodeInfoRequest.Types.Body { }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);
            var resp = await netmap_client.LocalNodeInfoAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new FormatException(nameof(LocalNodeInfo) + " invalid LocalNodeInfo response");
            return resp.MetaHeader.Epoch;
        }
    }
}

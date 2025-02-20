using System;
using System.Threading.Tasks;
using Grpc.Core;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Client
{
    public sealed class PutStream : IClientStream, IDisposable
    {
        public AsyncClientStreamingCall<PutRequest, PutResponse> Call { get; init; }

        public void Dispose()
        {
            Call?.Dispose();
        }

        public async Task Write(IRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (request is PutRequest putRequest)
            {
                if (putRequest.Body?.ObjectPartCase != PutRequest.Types.Body.ObjectPartOneofCase.Chunk) throw new ArgumentException("invalid requst type, expect chunk");
                await Call.RequestStream.WriteAsync(putRequest);
            }
            else
                throw new InvalidOperationException("invalid request type");
        }

        private void CheckStatus(IResponse resp)
        {
            var meta = resp.MetaHeader;
            if (meta?.Status is not null && !meta.Status.IsSuccess())
            {
                throw new RpcException(meta.Status.ToGrpcStatus());
            }
        }

        public async Task<IResponse> Close()
        {
            await Call.RequestStream.CompleteAsync();
            var resp = await Call.ResponseAsync;
            if (!resp.Verify())
                throw new InvalidOperationException("invalid object put response");
            CheckStatus(resp);
            return resp;
        }
    }
}

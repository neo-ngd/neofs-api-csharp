using Neo.FileSystem.API.Acl;
using Neo.FileSystem.API.Container;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsedSpaceAnnouncement = Neo.FileSystem.API.Container.AnnounceUsedSpaceRequest.Types.Body.Types.Announcement;

namespace Neo.FileSystem.API.Client
{
    public partial class Client
    {
        public async Task<Container.Container> GetContainer(CancellationToken context, ContainerID cid, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new GetRequest
            {
                Body = new GetRequest.Types.Body
                {
                    ContainerId = cid
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);

            var resp = await container_client.GetAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container get response");
            return resp.Body.Container;
        }

        public async Task<ContainerID> PutContainer(CancellationToken context, Container.Container container, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);

            container.Version = Refs.Version.SDKVersion();
            if (container.OwnerId is null) container.OwnerId = key.ToOwnerID_obsolete();
            var req = new PutRequest
            {
                Body = new PutRequest.Types.Body
                {
                    Container = container,
                    Signature = key.SignRFC6979(container),
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);
            var resp = await container_client.PutAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            return resp.Body.ContainerId;
        }

        public async Task DeleteContainer(CancellationToken context, ContainerID cid, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var body = new DeleteRequest.Types.Body
            {
                ContainerId = cid,
            };
            var req = new DeleteRequest();
            body.Signature = key.SignRFC6979(body);
            req.Body = body;
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);

            var resp = await container_client.DeleteAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
        }

        public async Task<List<ContainerID>> ListContainers(CancellationToken context, OwnerID owner, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new ListRequest
            {
                Body = new ListRequest.Types.Body
                {
                    OwnerId = owner
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);

            var resp = await container_client.ListAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            return resp.Body.ContainerIds.ToList();
        }

        public async Task<List<ContainerID>> ListSelfContainers(CancellationToken context, CallOptions options = null)
        {
            var w = key.ToOwnerID_obsolete();
            return await ListContainers(context, w, options);
        }

        public async Task<EAclWithSignature> GetEAclWithSignature(CancellationToken context, ContainerID cid, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new GetExtendedACLRequest
            {
                Body = new GetExtendedACLRequest.Types.Body
                {
                    ContainerId = cid
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);

            var resp = await container_client.GetExtendedACLAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            var eacl = resp.Body.Eacl;
            var sig = resp.Body.Signature;
            return new EAclWithSignature
            {
                Table = eacl,
                Signature = sig,
            };
        }

        public async Task<EACLTable> GetEACL(CancellationToken context, ContainerID cid, CallOptions options = null)
        {
            var eacl_with_sig = await GetEAclWithSignature(context, cid, options);
            if (!eacl_with_sig.Signature.VerifyRFC6979(eacl_with_sig.Table))
                throw new InvalidOperationException("invalid eacl signature");
            return eacl_with_sig.Table;
        }

        public async Task SetEACL(CancellationToken context, EACLTable eacl, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            eacl.Version = Refs.Version.SDKVersion();
            var req = new SetExtendedACLRequest
            {
                Body = new SetExtendedACLRequest.Types.Body
                {
                    Eacl = eacl,
                    Signature = key.SignRFC6979(eacl),
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);

            var resp = await container_client.SetExtendedACLAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
        }

        public async Task AnnounceContainerUsedSpace(CancellationToken context, List<UsedSpaceAnnouncement> announcements, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var body = new AnnounceUsedSpaceRequest.Types.Body();
            body.Announcements.AddRange(announcements);
            var req = new AnnounceUsedSpaceRequest
            {
                Body = body,
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);

            var resp = await container_client.AnnounceUsedSpaceAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid announce used space response");
        }
    }
}

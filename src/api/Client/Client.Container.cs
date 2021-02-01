using NeoFS.API.v2.Acl;
using NeoFS.API.v2.Container;
using NeoFS.API.v2.Cryptography;
using NeoFS.API.v2.Refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NeoFS.API.v2.Client
{
    public partial class Client
    {
        public Container.Container GetContainer(CancellationToken context, ContainerID cid, CallOptions options = null)
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
            req.SignRequest(key);

            var resp = container_client.Get(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container get response");
            return resp.Body.Container;
        }

        public ContainerID PutContainer(CancellationToken context, Container.Container container, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);

            container.Version = Refs.Version.SDKVersion();
            if (container.OwnerId is null) container.OwnerId = key.ToOwnerID();
            var req = new PutRequest
            {
                Body = new PutRequest.Types.Body
                {
                    Container = container,
                    Signature = container.SignMessagePart(key),
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            req.SignRequest(key);
            var resp = container_client.Put(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            return resp.Body.ContainerId;
        }

        public void DeleteContainer(CancellationToken context, ContainerID cid, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new DeleteRequest
            {
                Body = new DeleteRequest.Types.Body
                {
                    ContainerId = cid,
                }
            };
            req.Body.Signature = req.Body.SignMessagePart(key);
            req.MetaHeader = opts.GetRequestMetaHeader();
            req.SignRequest(key);

            var resp = container_client.Delete(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
        }

        public List<ContainerID> ListContainers(CancellationToken context, OwnerID owner, CallOptions options = null)
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
            req.SignRequest(key);

            var resp = container_client.List(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            return resp.Body.ContainerIds.ToList();
        }

        public List<ContainerID> ListSelfContainers(CancellationToken context, CallOptions options = null)
        {
            var w = key.ToOwnerID();
            return ListContainers(context, w, options);
        }

        public EAclWithSignature GetEAclWithSignature(CancellationToken context, ContainerID cid, CallOptions options = null)
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
            req.SignRequest(key);

            var resp = container_client.GetExtendedACL(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            var eacl = resp.Body.Eacl;
            var sig = resp.Body.Signature;
            if (!eacl.VerifyMessagePart(sig)) throw new InvalidOperationException("invalid eacl");
            return new EAclWithSignature
            {
                Table = eacl,
                Signature = sig,
            };
        }

        public EACLTable GetEACL(CancellationToken context, ContainerID cid, CallOptions options = null)
        {
            var eacl_with_sig = GetEAclWithSignature(context, cid, options);
            return eacl_with_sig.Table;
        }

        public void SetEACL(CancellationToken context, EACLTable eacl, CallOptions options = null)
        {
            var container_client = new ContainerService.ContainerServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new SetExtendedACLRequest
            {
                Body = new SetExtendedACLRequest.Types.Body
                {
                    Eacl = eacl,
                    Signature = eacl.SignMessagePart(key),
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            req.SignRequest(key);

            var resp = container_client.SetExtendedACL(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
        }
    }
}

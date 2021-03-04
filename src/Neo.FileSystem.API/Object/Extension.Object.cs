using Google.Protobuf;
using Neo.FileSystem.API.Refs;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Session;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace Neo.FileSystem.API.Object
{
    public partial class Object
    {
        public const int ChunkSize = 3 * (1 << 20);

        public Version Version => Header.Version;
        public ulong PayloadSize => Header.PayloadLength;
        public ContainerID ContainerID => Header.ContainerId;
        public OwnerID OwnerID => Header.OwnerId;
        public ulong CreationEpoch => Header.CreationEpoch;
        public Checksum PayloadChecksum => Header.PayloadHash;
        public Checksum PayloadHomomorphicHash => Header.HomomorphicHash;
        public List<Header.Types.Attribute> Attributes => Header.Attributes.ToList();
        public ObjectID PreviousID => Header.Split.Previous;
        public List<ObjectID> Children => Header.Split.Children.ToList();
        public SplitID SplitID => new SplitID(Header.Split.SplitId);
        public ObjectID ParentID => Header.Split.Parent;
        public SessionToken SessionToken => Header.SessionToken;
        public ObjectType ObjectType => Header.ObjectType;
        public bool HasParent => Header.Split != null;

        public Object Parent
        {
            get
            {
                var splitHeader = Header?.Split;
                if (splitHeader is null) return null;
                var parentSig = splitHeader.ParentSignature;
                var parentHeader = splitHeader.ParentHeader;
                if (parentSig is null || parentHeader is null)
                    return null;
                Object obj = new Object
                {
                    Header = parentHeader,
                    Signature = parentSig,
                };
                return obj;
            }
        }

        public ObjectID CalculateAndGetID
        {
            get
            {
                if (objectId_ is null)
                {
                    objectId_ = new ObjectID
                    {
                        Value = Header.Sha256()
                    };
                }
                return objectId_;
            }
        }

        private ObjectID CalculateID()
        {
            return new ObjectID
            {
                Value = Header.Sha256()
            };
        }

        public bool VerifyID()
        {
            return CalculateID() == ObjectId;
        }

        public Checksum CalculatePayloadChecksum()
        {
            if (Payload is null || Payload.Length == 0)
                throw new System.InvalidOperationException("cant payload checksum: invalid payload");
            return Payload.Sha256Checksum();
        }

        public bool VerifyPayloadChecksum()
        {
            return CalculatePayloadChecksum().Equals(Header?.PayloadHash);
        }

        public Signature CalculateIDSignature(ECDsa key)
        {
            return key.SignMessagePart(ObjectId);
        }

        public bool VerifyIDSignature()
        {
            return Signature.VerifyMessagePart(ObjectId);
        }

        public void SetVerificationFields(ECDsa key)
        {
            Header.PayloadHash = CalculatePayloadChecksum();
            ObjectId = CalculateID();
            Signature = CalculateIDSignature(key);
        }

        public bool CheckVerificationFields()
        {
            if (!VerifyIDSignature()) return false;
            if (!VerifyID()) return false;
            if (!VerifyPayloadChecksum()) return false;
            return true;
        }

        public Object CutPayload()
        {
            var obj = Parser.ParseFrom(this.ToByteArray());
            obj.payload_ = ByteString.Empty;
            return obj;
        }
    }
}

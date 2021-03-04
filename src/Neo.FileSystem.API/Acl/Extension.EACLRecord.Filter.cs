
namespace Neo.FileSystem.API.Acl
{
    public sealed partial class EACLRecord
    {
        public static partial class Types
        {
            public sealed partial class Filter
            {
                // ObjectFilterPrefix is a prefix of key to object header value or property.
                public const string ObjectFilterPrefix = "$Object:";

                // FilterObjectVersion is a filter key to "version" field of the object header.
                public const string FilterObjectVersion = ObjectFilterPrefix + "version";

                // FilterObjectID is a filter key to "object_id" field of the object.
                public const string FilterObjectID = ObjectFilterPrefix + "objectID";

                // FilterObjectContainerID is a filter key to "container_id" field of the object header.
                public const string FilterObjectContainerID = ObjectFilterPrefix + "containerID";

                // FilterObjectOwnerID is a filter key to "owner_id" field of the object header.
                public const string FilterObjectOwnerID = ObjectFilterPrefix + "ownerID";

                // FilterObjectCreationEpoch is a filter key to "creation_epoch" field of the object header.
                public const string FilterObjectCreationEpoch = ObjectFilterPrefix + "creationEpoch";

                // FilterObjectPayloadLength is a filter key to "payload_length" field of the object header.
                public const string FilterObjectPayloadLength = ObjectFilterPrefix + "payloadLength";

                // FilterObjectPayloadHash is a filter key to "payload_hash" field of the object header.
                public const string FilterObjectPayloadHash = ObjectFilterPrefix + "payloadHash";

                // FilterObjectType is a filter key to "object_type" field of the object header.
                public const string FilterObjectType = ObjectFilterPrefix + "objectType";

                // FilterObjectHomomorphicHash is a filter key to "homomorphic_hash" field of the object header.
                public const string FilterObjectHomomorphicHash = ObjectFilterPrefix + "homomorphicHash";

                // FilterObjectParent is a filter key to "split.parent" field of the object header.
                public const string FilterObjectParent = ObjectFilterPrefix + "split.parent";
            }
        }
    }
}

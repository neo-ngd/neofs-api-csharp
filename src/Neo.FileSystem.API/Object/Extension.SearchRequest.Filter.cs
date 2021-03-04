
namespace Neo.FileSystem.API.Object
{
    public partial class SearchRequest
    {
        public static partial class Types
        {
            public sealed partial class Body
            {
                public static partial class Types
                {
                    public sealed partial class Filter
                    {
                        // ReservedFilterPrefix is a prefix of key to object header value or property.
                        public const string ReservedFilterPrefix = "$Object:";


                        // FilterHeaderVersion is a filter key to "version" field of the object header.
                        public const string FilterHeaderVersion = ReservedFilterPrefix + "version";

                        // FilterHeaderObjectID is a filter key to "object_id" field of the object.
                        public const string FilterHeaderObjectID = ReservedFilterPrefix + "objectID";

                        // FilterHeaderContainerID is a filter key to "container_id" field of the object header.
                        public const string FilterHeaderContainerID = ReservedFilterPrefix + "containerID";

                        // FilterHeaderOwnerID is a filter key to "owner_id" field of the object header.
                        public const string FilterHeaderOwnerID = ReservedFilterPrefix + "ownerID";

                        // FilterHeaderCreationEpoch is a filter key to "creation_epoch" field of the object header.
                        public const string FilterHeaderCreationEpoch = ReservedFilterPrefix + "creationEpoch";

                        // FilterHeaderPayloadLength is a filter key to "payload_length" field of the object header.
                        public const string FilterHeaderPayloadLength = ReservedFilterPrefix + "payloadLength";

                        // FilterHeaderPayloadHash is a filter key to "payload_hash" field of the object header.
                        public const string FilterHeaderPayloadHash = ReservedFilterPrefix + "payloadHash";

                        // FilterHeaderObjectType is a filter key to "object_type" field of the object header.
                        public const string FilterHeaderObjectType = ReservedFilterPrefix + "objectType";

                        // FilterHeaderHomomorphicHash is a filter key to "homomorphic_hash" field of the object header.
                        public const string FilterHeaderHomomorphicHash = ReservedFilterPrefix + "homomorphicHash";

                        // FilterHeaderParent is a filter key to "split.parent" field of the object header.
                        public const string FilterHeaderParent = ReservedFilterPrefix + "split.parent";

                        // FilterHeaderParent is a filter key to "split.splitID" field of the object header.
                        public const string FilterHeaderSplitID = ReservedFilterPrefix + "split.splitID";

                        // FilterPropertyRoot is a filter key to check if regular object is on top of split hierarchy.
                        public const string FilterPropertyRoot = ReservedFilterPrefix + "ROOT";

                        // FilterPropertyPhy is a filter key to check if an object physically stored on a node.
                        public const string FilterPropertyPhy = ReservedFilterPrefix + "PHY";


                        // BooleanPropertyValueTrue is a true value for boolean property filters.
                        public const string BooleanPropertyValueTrue = "true";

                        // BooleanPropertyValueFalse is a false value for boolean property filters.
                        public const string BooleanPropertyValueFalse = "";
                    }
                }
            }
        }
    }
}

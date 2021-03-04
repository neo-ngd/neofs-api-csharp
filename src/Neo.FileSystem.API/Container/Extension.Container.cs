using Google.Protobuf;
using Neo.FileSystem.API.Refs;
using Neo.FileSystem.API.Cryptography;
using System;

namespace Neo.FileSystem.API.Container
{
    public partial class Container
    {
        // AttributeName is an attribute key that is commonly used to denote
        // human-friendly name.
        public const string AttributeName = "Name";

        // AttributeTimestamp is an attribute key that is commonly used to denote
        // user-defined local time of container creation in Unix Timestamp format.
        public const string AttributeTimestamp = "Timestamp";

        private ContainerID _id;
        public ContainerID CalCulateAndGetID
        {
            get
            {
                if (_id is null)
                    _id = new ContainerID
                    {
                        Value = this.Sha256()
                    };
                return _id;
            }
        }

        public Guid NonceUUID
        {
            get
            {
                return new Guid(nonce_.ToByteArray());
            }
            set
            {
                nonce_ = ByteString.CopyFrom(value.ToByteArray());
            }
        }

        public static partial class Types
        {
            public sealed partial class Attribute
            {
                // SysAttributePrefix is a prefix of key to system attribute.
                public const string SysAttributePrefix = "__NEOFS__";

                // SysAttributeSubnet is a string ID of container's storage subnet.
                public const string SysAttributeSubnet = SysAttributePrefix + "SUBNET";
            }
        }
    }
}
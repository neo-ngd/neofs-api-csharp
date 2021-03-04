namespace Neo.FileSystem.API.Object
{
    public sealed partial class Header
    {
        public static partial class Types
        {
            public sealed partial class Attribute
            {
                // SysAttributePrefix is a prefix of key to system attribute.
                public const string SysAttributePrefix = "__NEOFS__";

                // SysAttributeUploadID marks smaller parts of a split bigger object.
                public const string SysAttributeUploadID = SysAttributePrefix + "UPLOAD_ID";

                // SysAttributeExpEpoch tells GC to delete object after that epoch.
                public const string SysAttributeExpEpoch = SysAttributePrefix + "EXPIRATION_EPOCH";

                // AttributeName is an attribute key that is commonly used to denote
                // human-friendly name.
                public const string AttributeName = "Name";

                // AttributeFileName is an attribute key that is commonly used to denote
                // file name to be associated with the object on saving.
                public const string AttributeFileName = "FileName";

                // AttributeTimestamp is an attribute key that is commonly used to denote
                // user-defined local time of object creation in Unix Timestamp format.
                public const string AttributeTimestamp = "Timestamp";
            }
        }
    }
}

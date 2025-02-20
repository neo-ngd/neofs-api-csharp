using System;

namespace Neo.FileStorage.API.Refs
{
    public partial class Address
    {
        public Address(ContainerID cid, ObjectID oid)
        {
            ContainerId = cid;
            ObjectId = oid;
        }

        public string String()
        {
            return ContainerId.String() + "/" + ObjectId.String();
        }

        public static Address FromString(string address)
        {
            var parts = address.Split('/');
            if (parts.Length != 2) throw new FormatException("invalid object address string");
            var cid = ContainerID.FromString(parts[0]);
            var oid = ObjectID.FromString(parts[1]);
            return new Address
            {
                ContainerId = cid,
                ObjectId = oid,
            };
        }
    }
}

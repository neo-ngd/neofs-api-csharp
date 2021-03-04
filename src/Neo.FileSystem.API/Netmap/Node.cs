using Neo.FileSystem.API.Cryptography;
using System;
using System.Collections.Generic;

namespace Neo.FileSystem.API.Netmap
{
    public class Node : IEquatable<Node>, IComparable<Node>
    {
        public const string AttributeCapacity = "Capacity";
        public const string AttributePrice = "Price";
        public const string AttributeSubnet = "Subnet";
        public const string AttributeUNLOCODE = "UN-LOCODE";
        public const string AttributeCountryCode = "ConuntryCode";
        public const string AttributeCountry = "Country";
        public const string AttributeLocation = "Location";
        public const string AttributeSubDivCode = "SubDivCode";
        public const string AttributeSubDiv = "SubDiv";
        public const string AttributeContinent = "Continent";

        public ulong ID;
        public ulong Capacity;
        public ulong Price;
        public int Index;
        public NodeInfo Info;
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public double Weight;
        public ulong Distance;

        public ulong Hash => ID;
        public string NetworkAddress => Info.Address;
        public byte[] PublicKey => Info.PublicKey.ToByteArray();

        public Node(int index, NodeInfo ni)
        {
            ID = ni.PublicKey.ToByteArray().Murmur64(0);
            Index = index;
            Info = ni;
            foreach (var attr in ni.Attributes)
            {
                if (attr.Key == AttributeCapacity)
                    Capacity = ulong.Parse(attr.Value);
                else if (attr.Key == AttributePrice)
                    Price = ulong.Parse(attr.Value);
                Attributes.Add(attr.Key, attr.Value);
            }
        }

        public bool Equals(Node n)
        {
            return ID == n.ID;
        }

        public int CompareTo(Node n)
        {
            if (n == null)
                return 1;
            else
                return ID.CompareTo(n.ID);
        }
    }
}

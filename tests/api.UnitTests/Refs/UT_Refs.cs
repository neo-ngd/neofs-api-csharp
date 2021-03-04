using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Refs;
using Neo.FileSystem.API.Cryptography;
using System;

namespace Neo.FileSystem.API.UnitTests.TestRefs
{
    [TestClass]
    public class UT_Refs
    {
        [TestMethod]
        public void TestObjectID()
        {
            var id = ObjectID.FromSha256Bytes("a9aa4468861473c86e3d2db9d426e37e5858e015a678f7e6a94a12da3569c8b0".HexToBytes());
            Console.WriteLine(id);
        }

        [TestMethod]
        public void TestVersion()
        {
            var version = new Refs.Version
            {
                Major = 1,
                Minor = 1,
            };
            Console.WriteLine(version.ToByteArray().ToHexString());
            Console.WriteLine(version.ToString());
        }

        [TestMethod]
        public void TestOwnerID()
        {
            var version = new OwnerID
            {
                Value = ByteString.CopyFrom("351f694a2a49229f8e41d24542a0e6a7329b7ed065a113d002".HexToBytes()),
            };
            Console.WriteLine(version.ToByteArray().ToHexString());
        }
    }
}
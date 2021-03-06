

using System;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Object;

namespace Neo.FileStorage.API.UnitTests.TestObject
{
    [TestClass]
    public class UT_SplitID
    {
        [TestMethod]
        public void TestParse()
        {
            var sid = new SplitID();
            var str = sid.ToString();
            var sid1 = new SplitID();
            sid1.Parse(str);
            Assert.AreEqual(sid.ToString(), sid1.ToString());
        }

        [TestMethod]
        public void TestGuid()
        {
            var g = Guid.NewGuid();
            var sid = new SplitID();
            sid.SetGuid(g);
            Assert.AreEqual(g.ToString(), sid.ToString());
        }

        [TestMethod]
        public void TestNull()
        {
            SplitID sid1 = null;
            ByteString bs = null;
            SplitID sid2 = bs;
            sid2 = sid1;
        }
    }
}

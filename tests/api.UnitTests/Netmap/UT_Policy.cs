using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Netmap;

namespace Neo.FileSystem.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_Policy
    {
        [TestMethod]
        public void TestPlacementPolicyCBFWithEmptySelector()
        {
            var nodes = new Node[]{
                Helper.GenerateTestNode(0,("ID", "1"), ("Attr", "Same")),
                Helper.GenerateTestNode(1,("ID", "2"), ("Attr", "Same")),
                Helper.GenerateTestNode(2,("ID", "3"), ("Attr", "Same")),
                Helper.GenerateTestNode(3,("ID", "4"), ("Attr", "Same")),
            };
            var p1 = new PlacementPolicy(0, new Replica[] { new Replica(2, "") }, null, null);
            var p2 = new PlacementPolicy(3, new Replica[] { new Replica(2, "") }, null, null);
            var p3 = new PlacementPolicy(3, new Replica[] { new Replica(2, "X") }, new Selector[] { new Selector("X", "", Clause.Distinct, 2, "*") }, null);
            var p4 = new PlacementPolicy(3, new Replica[] { new Replica(2, "X") }, new Selector[] { new Selector("X", "Attr", Clause.Same, 2, "*") }, null);

            var nm = new NetMap(nodes);
            var v1 = nm.GetContainerNodes(p1, null);
            Assert.AreEqual(4, v1.Flatten().Count);
            var v2 = nm.GetContainerNodes(p2, null);
            Assert.AreEqual(4, v2.Flatten().Count);
            var v3 = nm.GetContainerNodes(p3, null);
            Assert.AreEqual(4, v3.Flatten().Count);
            var v4 = nm.GetContainerNodes(p4, null);
            Assert.AreEqual(4, v4.Flatten().Count);
        }
    }
}
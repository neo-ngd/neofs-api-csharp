using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Netmap;
using System;

namespace Neo.FileSystem.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_Clause
    {
        [TestMethod]
        public void TestString()
        {
            var c = Neo.FileSystem.API.Netmap.Clause.Same;
            Assert.AreEqual("Same", c.ToString());
        }
    }
}
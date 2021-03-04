using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileSystem.API.Netmap;
using System;

namespace Neo.FileSystem.API.UnitTests.TestNetmap
{
    public class FilterCase
    {
        public string Name;
        public bool Expect;
        public Filter F;
    }

    [TestClass]
    public class UT_Filter
    {
        [TestMethod]
        public void TestProcessFilter()
        {
            var filters = new Filter[]{
                new Filter
                {
                    Name = "StorageSSD",
                    Key = "Storage",
                    Value = "SSD",
                    Op = Operation.Eq
                },
                new Filter
                {
                    Name = "GoodRating",
                    Key = "Rating",
                    Value = "4",
                    Op = Operation.Ge
                },
                new Filter
                {
                    Name = "Main",
                    Key = "",
                    Value = "",
                    Op = Operation.And,
                },
            };
            filters[2].Filters.AddRange(new Filter[]{
                new Filter
                {
                    Name = "StorageSSD",
                    Key = "",
                    Value = "",
                    Op = 0
                },
                new Filter
                {
                    Name = "",
                    Key = "IntField",
                    Value = "123",
                    Op = Operation.Lt
                },
                new Filter
                {
                    Name = "GoodRating",
                    Key = "",
                    Value = "",
                    Op = 0
                }
            });
            var nm = new NetMap(null);
            var context = new Context(nm);
            var policy = new PlacementPolicy
            {
                ContainerBackupFactor = 1,
            };
            policy.Filters.AddRange(filters);
            context.ProcessFilters(policy);
            Assert.AreEqual(3, context.Filters.Count);
            foreach (var it in filters)
                Assert.AreEqual(it, context.Filters[it.Name]);
            Assert.AreEqual((UInt64)4, context.NumCache[filters[1]]);
            Assert.AreEqual((UInt64)123, context.NumCache[filters[2].Filters[1]]);
        }

        [TestMethod]
        public void TestProcessFiltersInvalid()
        {
            var fs = new Filter[]
            {
                new Filter
                {
                    Name = "",
                    Key = "Storage",
                    Value = "SSD",
                    Op = Operation.Eq,
                },
                new Filter
                {
                    Name = "Main",
                    Key = "",
                    Value = "",
                    Op = Operation.And,
                },
                new Filter
                {
                    Name = "Main",
                    Key = "Storage",
                    Value = "SSD",
                    Op = Operation.Eq,
                },
                new Filter
                {
                    Name = "Main",
                    Key = "Rating",
                    Value = "three",
                    Op = Operation.Ge,
                },
                new Filter
                {
                    Name = "Main",
                    Key = "Rating",
                    Value = "3",
                    Op = 0,
                },
                new Filter
                {
                    Name = "*",
                    Key = "Rating",
                    Value = "3",
                    Op = Operation.Ge,
                }
            };
            fs[1].Filters.Add(new Filter
            {
                Name = "StorageSSD",
                Key = "",
                Value = "",
                Op = 0,
            });
            fs[2].Filters.Add(new Filter
            {
                Name = "StorageSSD",
                Key = "",
                Value = "",
                Op = 0,
            });
            for (int i = 0; i < fs.Length; i++)
            {
                var c = new Context(new NetMap(null));
                var p = new PlacementPolicy
                {
                    ContainerBackupFactor = 1,
                };
                p.Filters.Add(fs[i]);
                if (i == 4)
                    Assert.ThrowsException<InvalidOperationException>(() => c.ProcessFilters(p));
                else if (i == 6)
                    Assert.ThrowsException<ArgumentNullException>(() => c.ProcessFilters(p));
                else
                    Assert.ThrowsException<ArgumentException>(() => c.ProcessFilters(p));
            }
        }

        [TestMethod]
        public void TestMatchSimple()
        {
            var ni = new NodeInfo();
            ni.Attributes.Add(new NodeInfo.Types.Attribute
            {
                Key = "Rating",
                Value = "4",
            });
            ni.Attributes.Add(new NodeInfo.Types.Attribute
            {
                Key = "Country",
                Value = "Germany",
            });
            var n = new Node(0, ni);
            var test_cases = new FilterCase[]
            {
                //#1
                new FilterCase
                {
                    Name = "GE_true",
                    Expect = true,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "4",
                        Op = Operation.Ge,
                    }
                },
                //#2
                new FilterCase
                {
                    Name = "GE_false",
                    Expect = false,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "5",
                        Op = Operation.Ge,
                    }
                },
                //#3
                new FilterCase
                {
                    Name = "GT_true",
                    Expect = true,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "3",
                        Op = Operation.Gt,
                    }
                },
                //#4
                new FilterCase
                {
                    Name = "GT_false",
                    Expect = false,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "4",
                        Op = Operation.Gt,
                    }
                },
                //#5
                new FilterCase
                {
                    Name = "LE_true",
                    Expect = true,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "4",
                        Op = Operation.Le,
                    }
                },
                //#6
                new FilterCase
                {
                    Name = "LE_false",
                    Expect = false,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "3",
                        Op = Operation.Le,
                    }
                },
                //#7
                new FilterCase
                {
                    Name = "LT_true",
                    Expect = true,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "5",
                        Op = Operation.Lt,
                    }
                },
                //#8
                new FilterCase
                {
                    Name = "LT_false",
                    Expect = false,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Rating",
                        Value = "4",
                        Op = Operation.Lt,
                    }
                },
                //#9
                new FilterCase
                {
                    Name = "EQ_true",
                    Expect = true,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Country",
                        Value = "Germany",
                        Op = Operation.Eq,
                    }
                },
                //#10
                new FilterCase
                {
                    Name = "EQ_false",
                    Expect = false,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Country",
                        Value = "China",
                        Op = Operation.Eq,
                    }
                },
                //#11
                new FilterCase
                {
                    Name = "NE_true",
                    Expect = true,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Country",
                        Value = "France",
                        Op = Operation.Ne,
                    }
                },
                //#12
                new FilterCase
                {
                    Name = "NE_false",
                    Expect = false,
                    F = new Filter
                    {
                        Name = "Main",
                        Key = "Country",
                        Value = "Germany",
                        Op = Operation.Ne,
                    }
                },
            };
            foreach (var t in test_cases)
            {
                var c = new Context(new NetMap(null));
                var p = new PlacementPolicy() { ContainerBackupFactor = 1 };
                p.Filters.Add(t.F);
                c.ProcessFilters(p);
                Assert.AreEqual(t.Expect, c.Match(t.F, n));
            }
        }

        [TestMethod]
        public void TestMatch()
        {
            var filters = new Filter[]
            {
                new Filter
                {
                    Name = "StorageSSD",
                    Key = "Storage",
                    Value = "SSD",
                    Op = Operation.Eq,
                },
                new Filter
                {
                    Name = "GoodRating",
                    Key = "Rating",
                    Value = "4",
                    Op = Operation.Ge,
                },
                new Filter("Main", "", "", Operation.And,
                    new Filter("StorageSSD", "", "", 0),
                    new Filter("", "IntField", "123", Operation.Lt),
                    new Filter("GoodRating", "", "", 0),
                    new Filter("", "", "", Operation.Or,
                        new Filter("", "Param", "Value1", Operation.Eq),
                        new Filter("", "Param", "Value2", Operation.Eq)
                    )
                )
            };
            var c = new Context(new NetMap(null));
            var p = new PlacementPolicy(1, null, null, filters);
            c.ProcessFilters(p);

            var n = Helper.GenerateTestNode(0, ("Storage", "SSD"), ("Rating", "10"), ("IntField", "100"), ("Param", "Value1"));
            Assert.IsTrue(c.ApplyFilter("Main", n));

            n = Helper.GenerateTestNode(0, ("Storage", "HDD"), ("Rating", "10"), ("IntField", "100"), ("Param", "Value1"));
            Assert.IsFalse(c.ApplyFilter("Main", n));

            n = Helper.GenerateTestNode(0, ("Storage", "SSD"), ("Rating", "3"), ("IntField", "100"), ("Param", "Value1"));
            Assert.IsFalse(c.ApplyFilter("Main", n));

            n = Helper.GenerateTestNode(0, ("Storage", "HDD"), ("Rating", "3"), ("IntField", "str"), ("Param", "Value1"));
            Assert.IsFalse(c.ApplyFilter("Main", n));

            n = Helper.GenerateTestNode(0, ("Storage", "HDD"), ("Rating", "3"), ("IntField", "100"), ("Param", "NotValue"));
            Assert.IsFalse(c.ApplyFilter("Main", n));
        }
    }
}

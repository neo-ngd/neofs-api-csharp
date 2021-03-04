using System.Collections.Generic;
using System;
using System.Linq;
using Neo.FileSystem.API.Netmap.Normalize;
using Neo.FileSystem.API.Netmap.Aggregator;

namespace Neo.FileSystem.API.Netmap
{
    public static class Helper
    {
        public static Func<Node, double> WeightFunc(INormalizer c, INormalizer p)
        {
            return n =>
            {
                return c.Normalize(n.Capacity) * p.Normalize(n.Price);
            };
        }

        public static Func<Node, double> GenarateWeightFunc(this List<Node> ns)
        {
            var mean = new MeanAgg();
            var min = new MinAgg();
            for (int i = 0; i < ns.Count; i++)
            {
                mean.Add(ns[i].Capacity);
                min.Add(ns[i].Price);
            }
            return WeightFunc(new SigmoidNorm(mean.Compute()), new ReverseMinNorm(min.Compute()));
        }

        public static ulong Distance(this UInt64 x, UInt64 y)
        {
            var acc = x ^ y;
            acc ^= acc >> 33;
            acc = acc * 0xff51afd7ed558ccd;
            acc ^= acc >> 33;
            acc = acc * 0xc4ceb9fe1a85ec53;
            acc ^= acc >> 33;
            return acc;
        }

        public static List<Node> Flatten(this List<List<Node>> ns)
        {
            return ns.Aggregate((ns1, ns2) => ns1.Concat(ns2).ToList());
        }

        public static int GetBucketCount(this Selector selector)
        {
            if (selector.Clause == Clause.Same)
            {
                return 1;
            }
            return (int)selector.Count;
        }

        public static int GetNodesInBucket(this Selector selector)
        {
            if (selector.Clause == Clause.Same)
            {
                return (int)selector.Count;
            }
            return 1;
        }
    }
}

using Neo.FileSystem.API.Netmap.Aggregator;
using Neo.FileSystem.API.Cryptography;
using System;
using System.Collections.Generic;

namespace Neo.FileSystem.API.Netmap
{
    public partial class Context
    {
        public const uint DefaultCBF = 3;
        public NetMap Map;
        public Dictionary<string, Filter> Filters = new Dictionary<string, Filter>();
        public Dictionary<string, Selector> Selectors = new Dictionary<string, Selector>();
        public Dictionary<string, List<List<Node>>> Selections = new Dictionary<string, List<List<Node>>>();
        public Dictionary<Filter, UInt64> NumCache = new Dictionary<Filter, ulong>();
        private byte[] pivot;
        private ulong pivotHash;
        private Func<IAggregator> newAggregator;
        private Func<Node, double> weightFunc;
        public uint cbf { get; private set; }

        public Context(NetMap map)
        {
            Map = map;
            newAggregator = () => new MeanIQRAgg(0);
            weightFunc = map.Nodes.GenarateWeightFunc();
            cbf = DefaultCBF;
        }

        public void SetPivot(byte[] pivot)
        {
            if (pivot is null || pivot.Length == 0) return;
            this.pivot = pivot;
            pivotHash = pivot.Murmur64(0);
        }

        public void SetWeightFunc(Func<Node, double> f)
        {
            weightFunc = f;
        }

        public void SetAggregator(Func<IAggregator> agg)
        {
            newAggregator = agg;
        }

        public void SetCBF(uint cbf)
        {
            if (cbf == 0)
                this.cbf = DefaultCBF;
            else
                this.cbf = cbf;
        }
    }
}



namespace Neo.FileSystem.API.Netmap.Aggregator
{
    public class MaxAgg : IAggregator
    {
        private double max = 0;

        public void Add(double n)
        {
            if (max < n)
                max = n;
        }

        public double Compute()
        {
            return max;
        }

        public void Clear()
        {
            max = 0;
        }
    }
}



namespace Neo.FileSystem.API.Netmap.Aggregator
{
    public class MinAgg : IAggregator
    {
        private double min;

        public void Add(double n)
        {
            if (min == 0 || n < min)
                min = n;
        }

        public double Compute()
        {
            return min;
        }

        public void Clear()
        {
            min = 0;
        }
    }
}



namespace Neo.FileSystem.API.Netmap.Aggregator
{
    public class MeanAgg : IAggregator
    {
        private double mean;
        private int count;

        public void Add(double n)
        {
            var c = count + 1;
            mean = mean * ((double)count / (double)c) + n / c;
            count++;
        }

        public double Compute()
        {
            return mean;
        }

        public void Clear()
        {
            mean = 0;
            count = 0;
        }
    }
}

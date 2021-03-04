

namespace Neo.FileSystem.API.Netmap.Aggregator
{
    public class MeanSumAgg : IAggregator
    {
        private double sum;
        private int count;

        public void Add(double n)
        {
            this.sum += n;
            this.count++;
        }

        public double Compute()
        {
            if (count == 0) return 0;
            return sum / count;
        }

        public void Clear()
        {
            sum = 0;
            count = 0;
        }
    }
}

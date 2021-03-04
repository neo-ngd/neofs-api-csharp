

namespace Neo.FileSystem.API.Netmap.Normalize
{
    public class MaxNorm : INormalizer
    {
        private double max;

        public MaxNorm(double max)
        {
            this.max = max;
        }

        public double Normalize(double w)
        {
            if (max == 0) return 0;
            return w / max;
        }
    }
}

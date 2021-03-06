

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public class ReverseMinNorm : INormalizer
    {
        private readonly double min;

        public ReverseMinNorm(double min)
        {
            this.min = min;
        }

        public double Normalize(double w)
        {
            if (w == 0) return 0;
            return min / w;
        }
    }
}

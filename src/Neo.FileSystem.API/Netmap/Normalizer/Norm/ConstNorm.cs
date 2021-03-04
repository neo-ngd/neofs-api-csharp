

namespace Neo.FileSystem.API.Netmap.Normalize
{
    public class ConstNorm : INormalizer
    {
        private double value;

        public ConstNorm(double value)
        {
            this.value = value;
        }

        public double Normalize(double w)
        {
            return value;
        }
    }
}

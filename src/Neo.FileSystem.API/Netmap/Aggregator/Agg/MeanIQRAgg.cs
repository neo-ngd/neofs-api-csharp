using System;
using System.Linq;

namespace Neo.FileSystem.API.Netmap.Aggregator
{
    public class MeanIQRAgg : IAggregator
    {
        private double k;
        private double[] arr = Array.Empty<double>();

        public MeanIQRAgg(double k)
        {
            this.k = k;
        }

        public void Add(double n)
        {
            arr = arr.Append(n).ToArray();
        }

        public double Compute()
        {
            if (arr.Length == 0)
                return 0;

            Array.Sort(arr);

            double min, max;
            if (arr.Length < 4)
            {
                max = arr[arr.Length - 1];
                min = arr[0];
            }
            else
            {
                var start = arr.Length / 4;
                var end = arr.Length * 3 / 4 - 1;
                var iqr = k * (arr[start] - arr[end]);
                max = arr[end] + iqr;
                min = arr[start] - iqr;
            }

            int count = 0;
            double sum = 0;

            foreach (var e in arr)
            {
                if (min <= e && e <= max)
                {
                    sum += e;
                    count++;
                }
            }

            return sum / count;
        }

        public void Clear()
        {
            arr = Array.Empty<double>();
        }
    }
}

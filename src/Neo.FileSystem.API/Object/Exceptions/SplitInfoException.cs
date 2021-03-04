using System;

namespace Neo.FileSystem.API.Object.Exceptions
{
    public class SplitInfoException : Exception
    {
        private readonly SplitInfo splitInfo;

        public SplitInfoException(SplitInfo si)
        {
            splitInfo = si;
        }

        public SplitInfo SplitInfo()
        {
            return splitInfo;
        }
    }
}

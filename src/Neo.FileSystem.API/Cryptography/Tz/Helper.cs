using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Neo.FileSystem.API.Cryptography.Tz
{
    public static class Helper
    {
        public static ulong NextUlong(this RandomNumberGenerator rng)
        {
            var buff = new byte[8];
            rng.GetBytes(buff);
            return BitConverter.ToUInt64(buff, 0);
        }

        public static int GetLeadingZeros(ulong value)
        {
            int i = 64;
            while (value != 0)
            {
                value >>= 1;
                i--;
            }
            return i;
        }

        public static int GetNonZeroLength(this ulong value)
        {
            return 64 - GetLeadingZeros(value);
        }
    }
}

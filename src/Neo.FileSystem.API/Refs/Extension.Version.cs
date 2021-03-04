
namespace Neo.FileSystem.API.Refs
{
    public partial class Version
    {
        public const uint SDKMajor = 2;
        public const uint SDKMinor = 0;

        public static Version SDKVersion()
        {
            return new Version
            {
                Major = SDKMajor,
                Minor = SDKMinor,
            };
        }

        public static bool IsSupportedVersion(Version ver)
        {
            if (ver.Major == SDKMajor && ver.Minor == SDKMinor)
                return true;
            return false;
        }
    }
}

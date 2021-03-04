using Neo.FileSystem.API.Acl;

namespace Neo.FileSystem.API.Session
{
    public static partial class SessionExtension
    {
        public static void SetVersion(this IRequestMeta meta, ulong epoch)
        {
            meta.MetaHeader.Epoch = epoch;
        }

        public static void SetTTL(this IRequestMeta meta, uint ttl)
        {
            meta.MetaHeader.Ttl = ttl;
        }

        public static void SetEpoch(this IRequestMeta meta, ulong epoch)
        {
            meta.MetaHeader.Epoch = epoch;
        }

        public static void AddXHeaders(this IRequestMeta meta, XHeader[] xHeaders)
        {
            meta.MetaHeader.XHeaders.AddRange(xHeaders);
        }

        public static void SetSessionToken(this IRequestMeta meta, SessionToken session)
        {
            meta.MetaHeader.SessionToken = session;
        }

        public static void SetBearerToken(this IRequestMeta meta, BearerToken bearer)
        {
            meta.MetaHeader.BearerToken = bearer;
        }
    }
}
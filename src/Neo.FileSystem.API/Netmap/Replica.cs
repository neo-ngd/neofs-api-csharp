
namespace Neo.FileSystem.API.Netmap
{
    public partial class Replica
    {
        public Replica(uint c, string s)
        {
            count_ = c;
            selector_ = s;
        }
    }
}


namespace Neo.FileSystem.API.Netmap
{
    public partial class Selector
    {
        public Selector(string name, string attr, Clause clause, uint count, string filter)
        {
            name_ = name;
            attribute_ = attr;
            clause_ = clause;
            count_ = count;
            filter_ = filter;
        }
    }
}

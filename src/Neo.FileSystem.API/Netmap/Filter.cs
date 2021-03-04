

namespace Neo.FileSystem.API.Netmap
{
    public partial class Filter
    {
        public Filter(string name, string key, string value, Operation op, params Filter[] sub_filters)
        {
            name_ = name;
            key_ = key;
            value_ = value;
            op_ = op;
            if (sub_filters != null) filters_.AddRange(sub_filters);
        }
    }
}

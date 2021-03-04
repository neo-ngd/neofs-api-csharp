
namespace Neo.FileSystem.API.Netmap
{
    public partial class PlacementPolicy
    {
        public PlacementPolicy(uint cbf, Replica[] replicas, Selector[] selectors, Filter[] filters)
        {
            containerBackupFactor_ = cbf;
            if (replicas != null) replicas_.AddRange(replicas);
            if (selectors != null) selectors_.AddRange(selectors);
            if (filters != null) filters_.AddRange(filters);
        }
    }
}

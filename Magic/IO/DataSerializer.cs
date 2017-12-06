using Magic.Net;

namespace Magic.IO
{
    public abstract class DataSerializer
    {
        public virtual DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            return ds;
        }
        public virtual DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds;
        }
    }
}

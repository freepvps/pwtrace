using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class PShopEntry : DataSerializer
    {
        public uint RoleId;
        public uint ShopType;
        public UnixTime CreateTime;
        public uint InvState;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            ShopType = ds.ReadUInt32();
            CreateTime = ds.Read<UnixTime>();
            InvState = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}

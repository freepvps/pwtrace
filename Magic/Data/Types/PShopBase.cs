using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class PShopBase : DataSerializer
    {
        public uint RoledId;
        public uint ShopType;

        public PShopItem[] BuyList;
        public PShopItem[] SellList;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoledId).
                Write(ShopType).
                Write(BuyList).
                Write(SellList);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoledId = ds.ReadUInt32();
            ShopType = ds.ReadUInt32();

            BuyList = ds.ReadArray<PShopItem>();
            SellList = ds.ReadArray<PShopItem>();

            return base.Deserialize(ds, vc);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class PShopItem : DataSerializer, IComparable<PShopItem>
    {
        public GRoleInventory Item;
        public uint Price;
        public int Reserved1;
        public int Reserved2;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(Item).
                Write(Price).
                Write(Reserved1).
                Write(Reserved2);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Item = ds.Read<GRoleInventory>();
            Price = ds.ReadUInt32();
            Reserved1 = ds.ReadInt32();
            Reserved2 = ds.ReadInt32();

            return base.Deserialize(ds, vc);
        }

        public int CompareTo(PShopItem item)
        {
            return Price.CompareTo(item.Price);
        }
    }
}

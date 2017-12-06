using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class EquipInfo : DataSerializer
    {

        public int ID;
        public int CellID;

        public int Count;
        public int MaxCount;

        public byte[] ItemData;

        public uint ProcType;
        public uint ExpireDate;

        public uint Unk1;
        public uint Unk2;

        public uint Mask;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ID = ds.ReadInt32();
            CellID = ds.ReadInt32();

            Count = ds.ReadInt32();
            MaxCount = ds.ReadInt32();

            ItemData = ds.ReadBytes();

            ProcType = ds.ReadUInt32();
            ExpireDate = ds.ReadUInt32();
            Unk1 = ds.ReadUInt32();
            Unk2 = ds.ReadUInt32();
            Mask = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}

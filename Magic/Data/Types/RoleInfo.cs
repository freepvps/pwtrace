using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class RoleInfo : DataSerializer
    {
        public RoleStatsInfo Stats;

        public int Experience;
        public int Spirit;

        public uint GuildId;

        public uint RoleId;

        public Gender Gender;
        public byte Race;
        public Occupation Occupation;

        public int Level;

        public int Unk;

        public string Name;
        public byte[] Face;

        public EquipInfo[] EquipList;

        public bool Selected;

        public UnixTime DeleteTime;
        public UnixTime CreateTime;
        public UnixTime LastOnline;

        public Point3F Position;

        public int WorldId;

        public byte[] CustomStatus;
        public byte[] CharacterMode;
        public uint RefererRole;
        public uint CashAdd;
        public byte[] ReincarnationData;
        public byte[] RealmData;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            Gender = ds.Read<Gender>(vc);

            Race = ds.ReadByte();
            Occupation = ds.Read<Occupation>(vc);

            Level = ds.ReadInt32();
            Unk = ds.ReadInt32();
            Name = ds.ReadUnicodeString();
            Face = ds.ReadBytes();

            try
            {
                EquipList = ds.ReadCollection<EquipInfo>(vc).ToArray();
                Selected = ds.ReadBoolean();

                DeleteTime = ds.Read<UnixTime>(vc);
                CreateTime = ds.Read<UnixTime>(vc);
                LastOnline = ds.Read<UnixTime>(vc);

                Position = ds.Read<Point3F>(vc);
                WorldId = ds.ReadInt32();

                /*
                CustomStatus = ds.ReadBytes();
                CharacterMode = ds.ReadBytes();
                RefererRole = ds.ReadUInt32();
                CashAdd = ds.ReadUInt32();
                ReincarnationData = ds.ReadBytes();
                RealmData = ds.ReadBytes();*/
            }
            catch
            {

            }

            return base.Deserialize(ds, vc);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

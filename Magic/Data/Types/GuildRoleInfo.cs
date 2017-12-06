using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class GuildRoleInfo : DataSerializer
    {
        public uint RoleId;
        public byte Level;
        public Occupation Occupation;
        public GuildStatus GuildStatus;
        public byte Unk1;
        public byte Unk2;
        public byte Unk3;
        public string Name;
        public string Title;
        public uint Score;
        public uint Unk4;
        public byte Unk5;
        public uint Reputation;
        public byte Unk6;
        public Gender Gender;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            Level = ds.ReadByte();
            Occupation = ds.Read<Occupation>();
            GuildStatus = ds.Read<GuildStatus>();
            Unk1 = ds.ReadByte();
            Unk2 = ds.ReadByte();
            Unk3 = ds.ReadByte();

            Name = ds.ReadUnicodeString();
            Title = ds.ReadUnicodeString();
            Score = ds.ReadUInt32();
            Unk4 = ds.ReadUInt32();
            Unk5 = ds.ReadByte();
            Reputation = ds.ReadUInt32();
            Unk6 = ds.ReadByte();
            Gender = ds.Read<Gender>();
            return base.Deserialize(ds, vc);
        }
    }
}

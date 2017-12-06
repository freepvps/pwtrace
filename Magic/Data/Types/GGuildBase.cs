using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class GGuildBase : DataSerializer
    {
        public uint GuildId;
        public string GuildName;

        public byte GuildLevel;
        public short MembersCount;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(GuildId).
                WriteUnicodeString(GuildName).

                Write(GuildLevel).
                Write(MembersCount);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            GuildId = ds.ReadUInt32();
            GuildName = ds.ReadUnicodeString();

            GuildLevel = ds.ReadByte();
            MembersCount = ds.ReadInt16();

            return base.Deserialize(ds, vc);
        }
    }
}

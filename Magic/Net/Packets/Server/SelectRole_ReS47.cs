using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x47, PacketType.ServerPacket)]
    public class SelectRole_ReS47 : GamePacket
    {
        public bool Unk1;
        public uint Unk2;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Unk1 = ds.ReadBoolean();
            Unk2 = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}

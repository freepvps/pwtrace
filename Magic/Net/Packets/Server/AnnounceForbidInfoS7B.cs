using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x7B, PacketType.ServerPacket)]
    public class AnnounceForbidInfoS7B : GamePacket
    {
        public uint AccountId;
        public uint Localsid;
        public GRoleForbid Forbid;
        public bool Disconnect;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            AccountId = ds.ReadUInt32();
            Localsid = ds.ReadUInt32();
            Forbid = ds.Read<GRoleForbid>(vc);
            Disconnect = ds.ReadBoolean();

            return base.Deserialize(ds, vc);
        }
    }
}

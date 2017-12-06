using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x5A, PacketType.ServerPacket)]
    public class KeepAliveS5A : GamePacket
    {
        public byte Type;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Type = ds.ReadByte();
            return base.Deserialize(ds, vc);
        }
    }
}

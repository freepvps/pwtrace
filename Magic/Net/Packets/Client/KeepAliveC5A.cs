using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x5A, PacketType.ClientPacket)]
    public class KeepAliveC5A : GamePacket
    {
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.PushBack(0x5A);
        }
    }
}

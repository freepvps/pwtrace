using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Net.Packets
{
    public enum PacketType : uint
    {
        // C
        ClientPacket = 0,
        // C22
        ClientContainer = 2,
        // C22-25
        ClientContainerC25 = 4,
        // S
        ServerPacket = 1,
        // S00-22
        ServerContainer = 3
    }
}

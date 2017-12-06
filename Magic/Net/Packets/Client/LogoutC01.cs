using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x01, PacketType.ClientContainer)]
    public class LogoutC01 : GamePacket
    {
        public LogoutC01() : this(1)
        {

        }
        public LogoutC01(int logoutType)
        {
            LogoutType = logoutType;
        }

        public int LogoutType;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.Write(LogoutType);
        }
    }
}

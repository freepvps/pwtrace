using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    public class BaseServerPacket : GamePacket
    {
        public DataStream DataStream { get; private set; }

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            DataStream = ds;
            return ds;
        }
    }
}

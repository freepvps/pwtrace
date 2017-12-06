using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x02, PacketType.ClientPacket)]
    public class CMKeyC02 : GamePacket<ConnectionInfo>
    {
        public byte[] CMKey;
        public bool Force;

        public CMKeyC02()
        {
        }
        public CMKeyC02(bool force)
        {
            Force = force;
        }

        protected internal override void HandleData(ConnectionInfo data)
        {
            CMKey = data.CMKey;
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(CMKey, true).
                Write(Force);
        }
    }
}

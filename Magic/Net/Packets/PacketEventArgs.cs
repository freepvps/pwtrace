using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Net.Packets
{
    public class PacketEventArgs : EventArgs
    {
        public PacketIdentifier PacketId { get; private set; }
        public GamePacket Packet { get; private set; }

        public PacketEventArgs(PacketIdentifier packetId, GamePacket gamePacket)
        {
            PacketId = packetId;
            Packet = gamePacket;
        }

        public override string ToString()
        {
            return PacketId.ToString();
        }
    }
}

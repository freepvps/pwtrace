using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x25, PacketType.ClientContainer)]
    public class ClientContainerC25 : GamePacket
    {
        private static BaseClientPacket emptyPacket = new BaseClientPacket();

        public uint PacketId { get; private set; }
        public GamePacket Packet { get; private set; }

        public ClientContainerC25(uint packetId) : this(packetId, emptyPacket)
        {

        }
        public ClientContainerC25(GamePacket gamePacket)
        {
            var packetId = GamePacket.GetOnePacketIdentifier(gamePacket);
            if (packetId.PacketType != PacketType.ClientContainerC25)
            {
                throw new ArgumentException("gamePacket is not a client container 25");
            }
            PacketId = packetId.PacketId;
            Packet = gamePacket;
        }
        public ClientContainerC25(uint packetId, GamePacket gamePacket)
        {
            PacketId = packetId;
            Packet = gamePacket;
        }

        protected internal override void HandleData(ConnectionData data)
        {
            Packet.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            ds.SaveEndianness();
            ds.IsLittleEndian = false;

            Packet.Serialize(ds, vc);
            ds.PushFront(EndianBitConverter.Little.GetBytes(ds.Count));
            ds.PushFront(EndianBitConverter.Little.GetBytes(PacketId));

            ds.RestoreEndianness();

            return ds;
        }
    }
}

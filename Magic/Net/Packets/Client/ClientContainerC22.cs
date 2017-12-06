using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x22, PacketType.ClientPacket)]
    public class ClientContainerC22 : GamePacket
    {
        private static BaseClientPacket emptyPacket = new BaseClientPacket();

        public uint PacketId { get; private set; }
        public GamePacket Packet { get; private set; }

        public ClientContainerC22(uint packetId) : this(packetId, emptyPacket)
        {

        }
        public ClientContainerC22(GamePacket gamePacket)
        {
            var packetId = GamePacket.GetOnePacketIdentifier(gamePacket);
            if (packetId.PacketType != PacketType.ClientContainer)
            {
                throw new ArgumentException("gamePacket is not a client container");
            }
            PacketId = packetId.PacketId;
            Packet = gamePacket;
        }
        public ClientContainerC22(uint packetId, GamePacket gamePacket)
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
            ds.IsLittleEndian = true;

            Packet.Serialize(ds, vc);
            ds.PushFront(EndianBitConverter.Little.GetBytes((ushort)PacketId));
            ds.PushFront(EndianBitConverter.Little.GetCompactUInt32Bytes((ushort)ds.Count));

            ds.RestoreEndianness();

            return base.Serialize(ds, vc);
        }
    }
}

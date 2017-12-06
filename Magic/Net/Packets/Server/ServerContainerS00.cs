using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x00, PacketType.ServerPacket)]
    public class ServerContainerS00 : GamePacket
    {
        List<PacketIdentifier> identifiers = new List<PacketIdentifier>();
        List<DataStream> streams = new List<DataStream>();

        public int Count
        {
            get
            {
                return identifiers.Count;
            }
        }
        public PacketIdentifier this[int index]
        {
            get
            {
                return identifiers[index];
            }
        }

        protected internal override void HandleData(ConnectionData data)
        {
            for (var i = 0; i < Count; i++)
            {
                var identifier = identifiers[i];
                var stream = streams[i];

                data.Host.Processor.ProcessServerPacketStream(identifier, stream, true);
            }
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ds.IsLittleEndian = true;
            while(ds.CanReadBytes(1))
            {
                var packetId = ds.ReadCompactUInt32();
                var packetLength = (int)ds.ReadCompactUInt32();

                if (packetId == 0x22)
                {
                    var containerStream = ds.ReadDataStream();
                    var containerId = containerStream.ReadUInt16();
                    containerStream.Flush();

                    identifiers.Add(new PacketIdentifier(containerId, PacketType.ServerContainer));
                    streams.Add(containerStream);
                }
                else
                {
                    var packetStream = new DataStream(ds.ReadBytes(packetLength));
                    packetStream.IsLittleEndian = false;
                    
                    identifiers.Add(new PacketIdentifier(packetId, PacketType.ServerPacket));
                    streams.Add(packetStream);
                }
            }

            return base.Deserialize(ds, vc);
        }
    }
}

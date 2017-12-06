using System;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x05, PacketType.ServerPacket)]
    public class ErrorInfoS05 : GamePacket
    {
        public uint ResultCode;
        public string Message;
        public ErrorInfoS05()
        {
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.ReadUInt32();
            Message = ds.ReadUnicodeString();
            return base.Deserialize(ds, vc);
        }
    }
}


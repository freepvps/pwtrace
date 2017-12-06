using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x01, PacketType.ServerPacket)]
    public class ServerInfoS01 : GamePacket<ConnectionInfo>
    {
        public byte[] Key;
        public ServerVersion ServerVersion;
        public byte AuthType;

        public int FreeCreationTime;
        public byte[] ServerAttribute;

        public string CRC;
        public byte? Bonus;
        public uint? UnknownId1;
        public uint? UnknownId2;

        public ServerVersion S01Version;
        public float ServerStatus;

        protected internal override void HandleData(ConnectionInfo data)
        {
            var serverAttribute = new byte[4];
            Buffer.BlockCopy(Key, 0, serverAttribute, 0, 4);
            ServerAttribute = serverAttribute;


            float serverStatus = Key[0] / 255f;
            ServerStatus = serverStatus;

            var ktime = new byte[4];
            ktime[0] = Key[3 + 4];
            ktime[1] = Key[2 + 4];
            ktime[2] = Key[1 + 4];
            ktime[3] = Key[0 + 4];

            FreeCreationTime = BitConverter.ToInt32(ktime, 0);

            data.ServerStatus = serverStatus;
            data.S01Key = Key;

            data.ServerVersion = ServerVersion;
            data.CRC = CRC;
            data.Bonus = Bonus ?? 0;

            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            S01Version = ServerVersion.Parse("1.2.6");

            Key = ds.ReadBytes();
            ServerVersion = ds.Read<ServerVersion>(vc);
            AuthType = ds.ReadByte();

            if (ds.CanReadBytes(1))
            {
                CRC = ds.ReadAsciiString();
                S01Version = ServerVersion.Parse("1.4.1");
            }
            if (ds.CanReadBytes(1))
            {
                Bonus = ds.ReadByte();
                S01Version = ServerVersion.Parse("1.4.2");
            }
            if (ds.CanReadBytes(4))
            {
                UnknownId1 = ds.ReadUInt32();
                S01Version = ServerVersion.Parse("1.5.5");
            }
            if (ds.CanReadBytes(4))
            {
                UnknownId2 = ds.ReadUInt32();
                S01Version = ServerVersion.Parse("1.5.5");
            }
            return ds;
        }
    }
}

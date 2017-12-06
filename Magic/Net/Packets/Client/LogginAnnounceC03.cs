using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x03, PacketType.ClientPacket, 1410)]
    public class LogginAnnounceC03 : GamePacket
    {
        static byte[] unkBytes = { 0xFF, 0xFF, 0xFF, 0xFF };

        public LogginAnnounceC03()
        {

        }
        public LogginAnnounceC03(string login, byte[] hash)
        {
            Login = login;
            Hash = hash;
        }

        public string Login;
        public byte[] Hash;
        public byte Unk = 0;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            ds.WriteAsciiString(Login);
            ds.Write(Hash, true);
            ds.Write(Unk);
            ds.Write(unkBytes, true);

            return base.Serialize(ds, vc);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Login = ds.ReadAsciiString();
            Hash = ds.ReadBytes();
            ds.ReadByte();
            ds.ReadBytes();

            return base.Deserialize(ds, vc);
        }
    }
}

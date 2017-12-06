using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x8F, PacketType.ServerPacket)]
    public class LastLoginInfoS8F : GamePacket<AccountInfo>
    {
        public uint AccountID;
        public uint UnkID;
        public UnixTime LastLoginTime;
        public byte[] LastLoginIP;
        public byte[] CurrentIP;

        protected internal override void HandleData(AccountInfo data)
        {
            data.LastLoginTime = LastLoginTime;
            data.LastLoginIP = IpToString(LastLoginIP);
            data.CurrentIP = IpToString(CurrentIP);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            AccountID = ds.ReadUInt32();
            UnkID = ds.ReadUInt32();
            LastLoginTime = ds.Read<UnixTime>(vc);
            LastLoginIP = ds.ReadBytes(4);
            CurrentIP = ds.ReadBytes(4);

            return base.Deserialize(ds, vc);
        }

        private string IpToString(byte[] ip)
        {
            return String.Format("{0}.{1}.{2}.{3}", ip[3], ip[2], ip[1], ip[0]);
        }
    }
}

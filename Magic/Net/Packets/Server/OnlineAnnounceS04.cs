using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x04, PacketType.ServerPacket)]
    public class OnlineAnnounceS04 : GamePacket<AccountInfo>
    {
        public uint AccountId;
        public uint UnkId;
        public UnixTime RemainTime;
        public byte ZoneId;
        public int? FreeTimeLeft;
        public int? FreeTimeEnd;
        public int? CreateTime;
        public bool? RefererFlag;
        public bool? PasswordFlag;
        public bool? Usbbind;
        public bool? AccountInfoFlag;

        protected internal override void HandleData(AccountInfo data)
        {
            data.AccountId = AccountId;
            data.UnkId = UnkId;
            data.RemainTime = RemainTime;
            data.ZoneId = ZoneId;
            data.SetRolesPage();
        }

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            AccountId = ds.ReadUInt32();
            UnkId = ds.ReadUInt32();
            RemainTime = ds.Read<UnixTime>();
            ZoneId = ds.ReadByte();
            if (ds.CanReadBytes(1)) FreeTimeLeft = ds.ReadInt32();
            if (ds.CanReadBytes(1)) FreeTimeEnd = ds.ReadInt32();
            if (ds.CanReadBytes(1)) CreateTime = ds.ReadInt32();
            if (ds.CanReadBytes(1)) RefererFlag = ds.ReadBoolean();
            if (ds.CanReadBytes(1)) PasswordFlag = ds.ReadBoolean();
            if (ds.CanReadBytes(1)) Usbbind = ds.ReadBoolean();
            if (ds.CanReadBytes(1)) AccountInfoFlag = ds.ReadBoolean();

            return base.Deserialize(ds, vc);
        }
    }
}

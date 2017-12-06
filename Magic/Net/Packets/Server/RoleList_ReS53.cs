using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x53, PacketType.ServerPacket)]
    public class RoleList_ReS53 : GamePacket<AccountInfo>
    {
        public int Unk1;
        public int NextSlot;
        public uint AccountID;
        public uint UnkID;
        public bool IsChar;
        public RoleInfo Role;

        protected internal override void HandleData(AccountInfo data)
        {
            if (IsChar)
            {
                data.AddRole(Role);
            }
        }

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Unk1 = ds.ReadInt32();
            NextSlot = ds.ReadInt32();
            AccountID = ds.ReadUInt32();
            UnkID = ds.ReadUInt32();

            IsChar = ds.ReadBoolean();
            if (IsChar)
            {
                Role = ds.Read<RoleInfo>(vc);
            }

            return base.Serialize(ds, vc);
        }
    }
}

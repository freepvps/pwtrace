using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x55, PacketType.ServerPacket)]
    public class CreateRole_ReS55 : GamePacket<AccountInfo>
    {
        public CreateRoleResultCode ResultCode;
        public uint AccountId;
        public uint UnkId;
        public RoleInfo Role;

        protected internal override void HandleData(AccountInfo data)
        {
            if(ResultCode.ResultCode == 0)
            {
                data.AddRole(Role);
            }
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.Read<CreateRoleResultCode>(vc);
            AccountId = ds.ReadUInt32();
            UnkId = ds.ReadUInt32();
            Role = ds.Read<RoleInfo>(vc);

            return base.Deserialize(ds, vc);
        }
    }
    public class CreateRoleResultCode : DataSerializer
    {
        public uint ResultCode;

        public override string ToString()
        {
            switch(ResultCode)
            {
                case 00: return "Персонаж успешно создан";
                case 25: return "Запрещено использовать такой ник";
                case 45: return "Ник уже используется";
                default: return "Unknown error: " + ResultCode;
            }
        }

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.ReadUInt32();
            return base.Deserialize(ds, vc);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.Write(ResultCode);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x46, PacketType.ClientPacket)]
    public class SelectRoleC46 : GamePacket<AccountInfo>
    {
        private int RoleIndex = -1;

        public uint RoleId;
        public byte Unk;

        public SelectRoleC46(uint roleId) : this(-1)
        {
            RoleId = roleId;
        }
        public SelectRoleC46(int roleIndex)
        {
            RoleIndex = roleIndex;
        }

        protected internal override void HandleData(AccountInfo data)
        {
            if (RoleIndex != -1)
            {
                RoleId = data.Roles[RoleIndex].RoleId;
            }
            foreach (var role in data.Roles)
            {
                if (role.RoleId == RoleId)
                {
                    data.SelectedRole = role;
                }
            }
            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(Unk);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x48, PacketType.ClientPacket)]
    public class EnterWorldC48 : GamePacket<AccountInfo>
    {
        public EnterWorldC48()
        {

        }
        public EnterWorldC48(uint roleId)
        {
            RoleId = roleId;
        }

        public uint RoleId;
        private static byte[] Unknown = new byte[20];

        protected internal override void HandleData(AccountInfo data)
        {
            if (data.SelectedRole != null)
            {
                RoleId = data.SelectedRole.RoleId;
            }
            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(Unknown);
        }
    }
}

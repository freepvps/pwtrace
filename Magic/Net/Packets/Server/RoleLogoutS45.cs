using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x45, PacketType.ServerPacket)]
    public class RoleLogoutS45 : GamePacket<AccountInfo>
    {
        public uint StatusCode;
        public uint RoleId;
        public uint ProviderLinkId;
        public uint ConnectionId;

        protected internal override void HandleData(AccountInfo data)
        {
            data.SetRolesPage();
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            StatusCode = ds.ReadUInt32();
            RoleId = ds.ReadUInt32();
            ProviderLinkId = ds.ReadUInt32();
            ConnectionId = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }

    }
}

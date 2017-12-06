using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x08, PacketType.ServerContainer)]
    public class PlayerPositionS08 : GamePacket<AccountInfo>
    {
        public int Experience;
        public int Spirit;
        public uint RoleId;
        public Point3F Position;

        protected internal override void HandleData(AccountInfo data)
        {
            data.SelectedRole.Experience = Experience;
            data.SelectedRole.Spirit = Spirit;

            data.EnteredWorld = true;
            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Experience = ds.ReadInt32();
            Spirit = ds.ReadInt32();
            RoleId = ds.ReadUInt32();
            Position = ds.Read<Point3F>(vc);

            return base.Deserialize(ds, vc);
        }
    }
}

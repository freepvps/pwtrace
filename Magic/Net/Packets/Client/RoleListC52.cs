using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x52, PacketType.ClientPacket)]
    public class RoleListC52 : GamePacket<AccountInfo>
    {
        public RoleListC52(int slot)
        {
            Slot = slot;
        }

        public uint AccountId;
        public int Unk;
        public int Slot;

        protected internal override void HandleData(AccountInfo data)
        {
            AccountId = data.AccountId;
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(AccountId).
                Write(Unk).
                Write(Slot);
        }
    }
}

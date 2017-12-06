using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class GRoomDetail : DataSerializer
    {
        public ushort RoomId;
        public string Title;
        public string Owner;
        public ushort Capacity;
        public byte Status;
        public GChatMember[] Members;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoomId = ds.ReadUInt16();
            Title = ds.ReadUnicodeString();
            Owner = ds.ReadUnicodeString();
            Capacity = ds.ReadUInt16();
            Status = ds.ReadByte();
            Members = ds.ReadArray<GChatMember>();
            return base.Deserialize(ds, vc);
        }
    }
}

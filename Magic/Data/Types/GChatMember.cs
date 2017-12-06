using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class GChatMember : DataSerializer
    {
        public uint RoleId;
        public string RoleName;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            RoleName = ds.ReadUnicodeString();
            return base.Deserialize(ds, vc);
        }
    }
}

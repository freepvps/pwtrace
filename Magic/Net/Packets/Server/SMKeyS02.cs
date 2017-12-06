using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;
using Magic.Net.Security;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x02, PacketType.ServerPacket)]
    public class SMKeyS02 : GamePacket<ConnectionInfo>
    {
        public byte[] SMKey;
        public bool Force;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            SMKey = ds.ReadBytes();
            Force = ds.ReadBoolean();

            return ds;
        }

        protected internal override void HandleData(ConnectionInfo data)
        {
            var CMKey = new byte[16];

            var rnd = new Random();
            rnd.NextBytes(CMKey);

            data.SMKey = SMKey;
            data.CMKey = CMKey;

            MD5Hash md5Hash = data.MD5;

            byte[] encKey = md5Hash.GetKey(SMKey);
            byte[] decKey = md5Hash.GetKey(CMKey);

            data.RC4Enc = new Rc4Encryption(encKey);
            data.RC4Dec = new Rc4Encryption(decKey);
            data.MPPC = new MppcUnpacker();
        }
    }
}

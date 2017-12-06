using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    public class BaseClientPacket : GamePacket
    {
        private static byte[] emptyBuffer = { };

        public byte[] Buffer { get; private set; }

        public BaseClientPacket() : this(emptyBuffer)
        {

        }
        public BaseClientPacket(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer == null");
            }

            Buffer = buffer;
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.PushBack(Buffer);
        }
    }
}

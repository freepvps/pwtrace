using System;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class GRoleForbid : DataSerializer
    {
        public byte Type;
        public int Time;
        public UnixTime Createtime;
        public string Reason;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(Type).
                Write(Time).
                Write(Createtime).
                WriteUnicodeString(Reason);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Type = ds.ReadByte();
            Time = ds.ReadInt32();
            Createtime = ds.Read<UnixTime>();
            Reason = ds.ReadUnicodeString();
            return ds;
        }

        public override string ToString()
        {
            return string.Format("{0} minutes with reason: {1}", Time / 60, Reason);
        }
    }
}
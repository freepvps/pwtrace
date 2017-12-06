using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class HpMp : DataSerializer
    {
        public int Value;
        public int Max;

        public decimal Percent
        {
            get
            {
                if (Max == 0)
                {
                    return 100;
                }

                return 100 * ((decimal)Value / (decimal)Max);
            }
        }

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Value = ds.ReadInt32();
            Max = ds.ReadInt32();
            return base.Deserialize(ds, vc);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.Write(Value).Write(Max);
        }
        public override string ToString()
        {
            return string.Format("{0}/{1} ({2}%)", Value, Max, Percent);
        }
    }
}

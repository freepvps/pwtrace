using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.IO
{
    public class VersionControl
    {
        private static VersionControl any = new VersionControl(int.MinValue, int.MaxValue);
        public static VersionControl Any
        {
            get
            {
                return any;
            }
        }

        public int MinVersion { get; set; }
        public int MaxVersion { get; set; }

        public VersionControl(int minVersion) : this(minVersion, int.MaxValue)
        {

        }
        public VersionControl(int minVersion, int maxVersion)
        {
            MinVersion = minVersion;
            MaxVersion = maxVersion;
        }
        
        public bool Check(int minVersion)
        {
            return minVersion <= MaxVersion;
        }
        public bool Check(int minVersion, int maxVersion)
        {
            var min = Math.Max(MinVersion, minVersion);
            var max = Math.Min(MaxVersion, maxVersion);

            return min <= max;
        }
        public bool Check(VersionControl vc)
        {
            var min = Math.Max(MinVersion, vc.MinVersion);
            var max = Math.Min(MaxVersion, vc.MaxVersion);

            return min <= max;
        }
    }
}

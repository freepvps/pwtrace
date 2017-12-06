using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Net.Packets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketGroup : Attribute
    {
        public string GroupName { get; private set; }
        public PacketGroup()
        {
            GroupName = string.Empty;
        }
        public PacketGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException("Argument is null or empty");
            }
            GroupName = groupName;
        }
    }
}

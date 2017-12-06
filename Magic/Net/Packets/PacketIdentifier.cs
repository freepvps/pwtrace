using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PacketIdentifier : Attribute
    {
        public uint PacketId { get; private set; }
        public PacketType PacketType { get; private set; }
        public VersionControl Version { get; private set; }
        
        public PacketIdentifier(uint packetId, PacketType packetType) : this(packetId, packetType, VersionControl.Any)
        {
        }
        public PacketIdentifier(uint packetId, PacketType packetType, int version) : this(packetId, packetType, new VersionControl(version))
        {
        }
        public PacketIdentifier(uint packetId, PacketType packetType, int minVersion, int maxVersion) : this(packetId, packetType, new VersionControl(minVersion, maxVersion))
        {
        }
        public PacketIdentifier(uint packetId, PacketType packetType, VersionControl version)
        {
            PacketId = packetId;
            PacketType = packetType;
            Version = version;
        }

        public override bool Equals(object obj)
        {
            if (obj is PacketIdentifier)
            {
                var pid = obj as PacketIdentifier;

                return pid.PacketId == PacketId && pid.PacketType == PacketType;
            }
            else
            {
                return base.Equals(obj);
            }
        }
        public override int GetHashCode()
        {
            return (int)(PacketId ^ ((uint)PacketType << 16));
        }
        public override string ToString()
        {
            string prefix = string.Empty;
            switch (PacketType)
            {
                case PacketType.ClientContainer:    prefix = "c22-"; break;
                case PacketType.ServerContainer:    prefix = "s00-"; break;
                case PacketType.ClientContainerC25: prefix = "c25-"; break;
                case PacketType.ClientPacket:       prefix = "c"; break;
                case PacketType.ServerPacket:       prefix = "s"; break;
            }

            return prefix + PacketId.ToString("X2");
        }
    }
}

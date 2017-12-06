using System;
using System.Collections.Generic;
using System.Reflection;
using Magic.IO;

namespace Magic.Net.Packets
{
    public class PacketsRegistry
    {
        private static string[] emptyGroups = { };
        private static string[] globalGroups = { string.Empty };
        private static Type[] emptyTypes = { };
        private static object[] emptyObjects = { };
        private static Assembly[] currentAssembly = { Assembly.GetExecutingAssembly() };
        
        public static PacketsRegistry Create()
        {
            return Create(VersionControl.Any);
        }
        public static PacketsRegistry Create(VersionControl versionControl)
        {
            return Create(versionControl, true);
        }
        public static PacketsRegistry Create(VersionControl versionControl, bool includeGlobal)
        {
            return Create(versionControl, includeGlobal, emptyGroups);
        }
        public static PacketsRegistry Create(VersionControl versionControl, bool includeGlobal, params string[] groups)
        {
            return Create(versionControl, currentAssembly, includeGlobal, groups);
        }
        public static PacketsRegistry Create(VersionControl versionControl, Assembly[] assemblies, bool includeGlobal, params string[] groups)
        {
            var registry = new PacketsRegistry(versionControl);
            registry.Register(assemblies, includeGlobal, groups);
            return registry;
        }

        // ----------------------------
        private Dictionary<PacketIdentifier, Type> packets = new Dictionary<PacketIdentifier, Type>();
        public VersionControl Version { get; private set; }

        public PacketsRegistry() : this(VersionControl.Any)
        {

        }
        public PacketsRegistry(VersionControl version)
        {
            Version = version;
        }
        public PacketsRegistry(Assembly[] assemblies) : this(assemblies, VersionControl.Any)
        {
        }
        public PacketsRegistry(Assembly[] assemblies, VersionControl version)
        {
            Version = version;
            Register(assemblies, true, emptyGroups);
        }
        public PacketsRegistry(IEnumerable<Type> types) : this(types, VersionControl.Any)
        {
        }
        public PacketsRegistry(IEnumerable<Type> types, VersionControl version)
        {
            Version = version;
            Register(types);
        }

        public void Register()
        {
            Register(true, emptyGroups);
        }
        public void Register(params string[] groups)
        {
            Register(true, groups);
        }
        public void Register(bool includeGlobal)
        {
            Register(includeGlobal, emptyGroups);
        }
        public void Register(bool includeGlobal, params string[] groups)
        {
            Register(currentAssembly, includeGlobal, groups);
        }
        public void Register(Assembly[] assemblies, bool includeGlobal, params string[] groups)
        {
            var types = new Dictionary<string, HashSet<Type>>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (GamePacket.IsGamePacket(type))
                    {
                        var packetGroups = GamePacket.GetPacketGroups(type);
                        if (packetGroups.Length == 0)
                        {
                            packetGroups = globalGroups;
                        }

                        foreach (var group in packetGroups)
                        {
                            HashSet<Type> set;
                            if (!types.TryGetValue(group, out set))
                            {
                                set = new HashSet<Type>();
                                types[group] = set;
                            }
                            set.Add(type);
                        }
                    }
                }
            }
            
            if (includeGlobal)
            {
                HashSet<Type> set;
                if (types.TryGetValue(string.Empty, out set))
                {
                    Register(set);
                }
            }
            foreach (var group in groups)
            {
                HashSet<Type> set;
                if (types.TryGetValue(group, out set))
                {
                    Register(set);
                }
            }
        }
        public void Register(IEnumerable<Type> types)
        {
            foreach(var type in types)
            {
                Register(type);
            }
        }
        public void Register<T>() where T : GamePacket
        {
            Register(typeof(T));
        }
        public void Register(Type type)
        {
            var packetIds = GamePacket.GetPacketIdentifiers(type);
            foreach(var packetId in packetIds)
            {
                Register(type, packetId);
            }
        }
        public void Register(Type type, uint packetId, PacketType packetType)
        {
            Register(type, new PacketIdentifier(packetId, packetType));
        }
        public void Register(Type type, PacketIdentifier packetId)
        {
            if (!GamePacket.IsGamePacket(type) || !packetId.Version.Check(Version))
            {
                return;
            }
            
            packets[packetId] = type;
        }
        public GamePacket GetPacket(uint packetId, PacketType packetType)
        {
            return GetPacket(new PacketIdentifier(packetId, packetType));
        }
        public GamePacket GetPacket(PacketIdentifier packetId)
        {
            return GetPacketType(packetId).GetConstructor(emptyTypes).Invoke(emptyObjects) as GamePacket;
        }
        public bool TryGetPacket(uint packetId, PacketType packetType, out GamePacket gamePacket)
        {
            return TryGetPacket(new PacketIdentifier(packetId, packetType), out gamePacket);
        }
        public bool TryGetPacket(PacketIdentifier packetId, out GamePacket gamePacket)
        {
            Type type;
            if (TryGetPacketType(packetId, out type))
            {
                gamePacket = type.GetConstructor(emptyTypes).Invoke(emptyObjects) as GamePacket;
                return true;
            }
            else
            {
                gamePacket = null;
                return false;
            }
        }

        public Type GetPacketType(uint packetId, PacketType packetType)
        {
            return GetPacketType(new PacketIdentifier(packetId, packetType));
        }
        public Type GetPacketType(PacketIdentifier packetId)
        {
            Type type;
            if (!TryGetPacketType(packetId, out type))
            {
                throw new Exception("Unknown packet");
            }
            return type;
        }

        public bool TryGetPacketType(uint packetId, PacketType packetType, out Type type)
        {
            return TryGetPacketType(new PacketIdentifier(packetId, packetType), out type);
        }
        public bool TryGetPacketType(PacketIdentifier packetId, out Type type)
        {
            return packets.TryGetValue(packetId, out type);
        }

        public bool Contains(uint packetId, PacketType packetType)
        {
            return Contains(new PacketIdentifier(packetId, packetType));
        }
        public bool Contains(PacketIdentifier packetId)
        {
            return packets.ContainsKey(packetId);
        }

        public void Remove<T>() where T : GamePacket
        {
            RemoveRange(GamePacket.GetPacketIdentifiers<T>());
        }
        public void Remove(PacketIdentifier packetId)
        {
            packets.Remove(packetId);
        }
        public void RemoveRange(PacketIdentifier[] packetIds)
        {
            foreach(var packetId in packetIds)
            {
                packets.Remove(packetId);
            }
        }
    }
}

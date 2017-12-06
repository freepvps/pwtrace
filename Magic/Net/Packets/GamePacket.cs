using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;

namespace Magic.Net.Packets
{
    public class GamePacket : DataSerializer
    {
        internal protected virtual void HandleData(ConnectionData data)
        {

        }

        // Extenssions
        private static Type gamePacketType = typeof(GamePacket);
        private static Type packetIdType = typeof(PacketIdentifier);
        private static Type packetGroupType = typeof(PacketGroup);
        
        public static bool IsGamePacket(Type type)
        {
            return type.IsSubclassOf(gamePacketType);
        }
        private static void ValidateGamePacket(Type type)
        {
            if (!IsGamePacket(type))
            {
                throw new Exception("Is no packet");
            }
        }
        
        public static PacketIdentifier GetOnePacketIdentifier<T>() where T : GamePacket
        {
            return GetOnePacketIdentifier(typeof(T));
        }
        public static PacketIdentifier GetOnePacketIdentifier(GamePacket gamePacket)
        {
            return GetOnePacketIdentifier(gamePacket.GetType());
        }
        public static PacketIdentifier GetOnePacketIdentifier(Type type)
        {
            ValidateGamePacket(type);

            var identifiers = GamePacket.GetPacketIdentifiers(type);
            if (identifiers.Length != 1)
            {
                throw new Exception("Can't select one packet identifier");
            }

            return identifiers[0];
        }
        public static PacketIdentifier[] GetPacketIdentifiers<T>() where T : GamePacket
        {
            return GetPacketIdentifiers(typeof(T));
        }
        public static PacketIdentifier[] GetPacketIdentifiers(GamePacket gamePacket)
        {
            return GetPacketIdentifiers(gamePacket.GetType());
        }
        public static PacketIdentifier[] GetPacketIdentifiers(Type type)
        {
            ValidateGamePacket(type);

            var attributes = type.GetCustomAttributes(typeof(PacketIdentifier), false);
            return (attributes as PacketIdentifier[]);
        }

        public static string[] GetPacketGroups<T>() where T : GamePacket
        {
            return GetPacketGroups(typeof(T));
        }
        public static string[] GetPacketGroups(GamePacket gamePacket)
        {
            return GetPacketGroups(gamePacket.GetType());
        }
        public static string[] GetPacketGroups(Type type)
        {
            ValidateGamePacket(type);

            var attributes = type.GetCustomAttributes(typeof(PacketGroup), false);
            var groups = new string[attributes.Length];
            for (var i = 0; i < attributes.Length; i++)
            {
                groups[i] = (attributes[i] as PacketGroup).GroupName;
            }
            return groups;
        }
    }
    public class GamePacket<T> : GamePacket where T : DataBlock
    {
        internal protected override void HandleData(ConnectionData data)
        {
            T dataBlock;
            if (data.TryGetDataBlock<T>(out dataBlock))
            {
                HandleData(dataBlock);
            }

            base.HandleData(data);
        }
        internal protected virtual void HandleData(T data)
        {
        }
    }
}
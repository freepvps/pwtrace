using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x54, PacketType.ClientPacket)]
    public class CreateRoleC54 : GamePacket<AccountInfo>
    {
        static byte[] ConstUnkData = new byte[512];

        public CreateRoleC54(string name, Gender gender, Occupation occupation, byte[] face)
        {
            Name = name;
            Gender = gender;
            Occupation = occupation;
            Face = face;

            UnkData = ConstUnkData;
        }

        public uint AccountId;
        public uint Unk1;
        public uint Unk2 = 0xFFFFFFFF;
        public Gender Gender;
        public byte Race;
        public Occupation Occupation;
        public uint Level;
        public uint Unk3;
        public string Name;
        public byte[] Face;
        public byte[] UnkData;

        protected internal override void HandleData(AccountInfo data)
        {
            AccountId = data.AccountId;

            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(AccountId).
                Write(Unk1).
                Write(Unk2).
                Write(Gender).
                Write(Race).
                Write(Occupation).
                Write(Level).
                Write(Unk3).
                WriteUnicodeString(Name).
                Write(Face, true).
                Write(UnkData, false);
        }
    }
}

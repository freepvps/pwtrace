using Magic.IO;

namespace Magic.Data.Types
{
    public class Gender : DataSerializer
    {
        private static Gender male = new Gender(0);
        private static Gender female = new Gender(1);

        public static Gender Male { get { return male; } }
        public static Gender Female { get { return female; } }

        public byte GenderId;

        public Gender()
        {
        }
        public Gender(byte gender)
        {
            GenderId = gender;
        }

        public override string ToString()
        {
            return GenderId >= 1 ? "Женский" : "Мужской";
        }
        public string ToShortString()
        {
            return GenderId >= 1 ? "Ж" : "М";
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            ds.Write(GenderId);
            return base.Serialize(ds, vc);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            GenderId = ds.ReadByte();
            return base.Deserialize(ds, vc);
        }
    }
}

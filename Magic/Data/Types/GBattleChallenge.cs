using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class GBattleChallenge : DataSerializer
    {
        public MapId MapId;
        public uint Challenger;
        public uint Deposit;
        public uint Bonus;
        public UnixTime EndTime;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            MapId = ds.Read<MapId>();
            Challenger = ds.ReadUInt32();
            Deposit = ds.ReadUInt32();
            Bonus = ds.ReadUInt32();
            EndTime = ds.Read<UnixTime>(vc);

            return base.Deserialize(ds, vc);
        }
    }
}

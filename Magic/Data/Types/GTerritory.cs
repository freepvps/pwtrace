using System;
using System.Text;
using Magic.IO;

namespace Magic.Data.Types
{
    public class GTerritory : DataSerializer
    {
        /*
    <rpcdata name="GTerritory">
        <variable name="id" type="char" default="0"/>
        <variable name="level" type="char" default="0"/>
        <variable name="color" type="char" default="0"/>
        <variable name="owner" type="unsigned int" default="0"/>
        <variable name="challenger" type="unsigned int" default="0"/>
        <variable name="battle_time" type="unsigned int" default="0"/>
        <variable name="deposit" type="int" default="0"/>
        <variable name="maxbonus" type="int" default="0"/>
    </rpcdata>
         */

        public MapId Id;
        public byte Level;
        public byte Color;
        public uint Owner;
        public uint Challenger;
        public UnixTime BattleTime;
        public int Deposit;
        public int MaxBonus;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write((byte)Id.Id).
                Write(Level).
                Write(Color).
                Write(Owner).
                Write(Challenger).
                Write(BattleTime).
                Write(Deposit).
                Write(MaxBonus);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Id = new MapId(ds.ReadByte());
            Level = ds.ReadByte();
            Color = ds.ReadByte();
            Owner = ds.ReadUInt32();
            Challenger = ds.ReadUInt32();
            BattleTime = ds.Read<UnixTime>(vc);
            Deposit = ds.ReadInt32();
            MaxBonus = ds.ReadInt32();

            return base.Deserialize(ds, vc);
        }
    }
}


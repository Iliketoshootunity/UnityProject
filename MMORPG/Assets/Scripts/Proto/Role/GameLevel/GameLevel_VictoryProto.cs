using System.Collections;
using System.Collections.Generic;

public class GameLevel_VictoryProto : IProto
{

    public class MonsterItem
    {
        public int MonsterId;
        public int MonsterCount;
    }

    public class ReceiveGoodsItem
    {
        public byte GoodsType;
        public int GoodsId;
        public int GoodsCount;
    }


    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_VictroyProto;
        }
    }

    public int GameLevelId;

    public byte Grade;

    public byte Star;

    public int Exp;

    public int Gold;

    public int KillMonsterCount;

    public List<MonsterItem> KillMonsterList = new List<MonsterItem>();

    public int ReceiveGoodsCount;

    public List<ReceiveGoodsItem> ReceiveGoodsList = new List<ReceiveGoodsItem>();


    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(GameLevelId);
            ms.WriteByte(Grade);
            ms.WriteByte(Star);
            ms.WriteInt(Exp);
            ms.WriteInt(Gold);
            KillMonsterCount = KillMonsterList.Count;
            ms.WriteInt(KillMonsterCount);
            for (int i = 0; i < KillMonsterCount; i++)
            {
                ms.WriteInt(KillMonsterList[i].MonsterId);
                ms.WriteInt(KillMonsterList[i].MonsterCount);
            }
            ReceiveGoodsCount = ReceiveGoodsList.Count;
            ms.WriteInt(ReceiveGoodsCount);
            for (int i = 0; i < ReceiveGoodsCount; i++)
            {
                ms.WriteByte(ReceiveGoodsList[i].GoodsType);
                ms.WriteInt(ReceiveGoodsList[i].GoodsId);
                ms.WriteInt(ReceiveGoodsList[i].GoodsCount);
            }
            return ms.ToArray();
        }
    }

    public static GameLevel_VictoryProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_VictoryProto proto = new GameLevel_VictoryProto();
            proto.GameLevelId = ms.ReadInt();
            proto.Grade = (byte)ms.ReadByte();
            proto.Star = (byte)ms.ReadByte();
            proto.Exp = ms.ReadInt();
            proto.Gold = ms.ReadInt();
            proto.KillMonsterCount = ms.ReadInt();
            for (int i = 0; i < proto.KillMonsterCount; i++)
            {
                MonsterItem monsterItem = new MonsterItem();
                monsterItem.MonsterId = ms.ReadInt();
                monsterItem.MonsterCount = ms.ReadInt();
                proto.KillMonsterList.Add(monsterItem);
            }
            proto.ReceiveGoodsCount = ms.ReadInt();
            for (int i = 0; i < proto.ReceiveGoodsCount; i++)
            {
                ReceiveGoodsItem goodsItem = new ReceiveGoodsItem();
                goodsItem.GoodsType = (byte)ms.ReadByte();
                goodsItem.GoodsId = ms.ReadInt();
                goodsItem.GoodsCount = ms.ReadInt();
                proto.ReceiveGoodsList.Add(goodsItem);
            }
            return proto;
        }
    }
}

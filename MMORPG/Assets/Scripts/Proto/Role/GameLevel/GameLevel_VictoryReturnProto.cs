using System.Collections;
using System.Collections.Generic;


public class GameLevel_VictoryReturnProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_VictroyReturnProto;
        }
    }

    public bool IsSucess;
    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteBool(IsSucess);
            return ms.ToArray();
        }

    }

    public static GameLevel_VictoryReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_VictoryReturnProto proto = new GameLevel_VictoryReturnProto();
            proto.IsSucess = ms.ReadBool();
            return proto;
        }
    }

}

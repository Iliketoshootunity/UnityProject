using System.Collections;
using System.Collections.Generic;


public class GameLevel_FailReturnProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_FailReturnProto;
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

    public static GameLevel_FailReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_FailReturnProto proto = new GameLevel_FailReturnProto();
            proto.IsSucess = ms.ReadBool();
            return proto;
        }
    }

}

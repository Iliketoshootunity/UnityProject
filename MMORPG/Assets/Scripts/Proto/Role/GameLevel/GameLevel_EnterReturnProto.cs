using System.Collections;
using System.Collections.Generic;


public class GameLevel_EnterReturnProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_EnterReturnProto;
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

    public static GameLevel_EnterReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_EnterReturnProto proto = new GameLevel_EnterReturnProto();
            proto.IsSucess = ms.ReadBool();
            return proto;
        }
    }
}

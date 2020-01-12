using System.Collections;
using System.Collections.Generic;

public class GameLevel_ResurgenceReturnProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_ResurgenceReturnProto;
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

    public static GameLevel_ResurgenceReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_ResurgenceReturnProto proto = new GameLevel_ResurgenceReturnProto();
            proto.IsSucess = ms.ReadBool();
            return proto;
        }
    }
}

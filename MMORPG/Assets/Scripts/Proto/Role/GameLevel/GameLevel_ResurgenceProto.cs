using System.Collections;
using System.Collections.Generic;


public class GameLevel_ResurgenceProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_ResurgenceProto;
        }
    }

    public int GameLevelId;

    public byte Grade;
    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            return ms.ToArray();
        }

    }

    public static GameLevel_ResurgenceProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_ResurgenceProto proto = new GameLevel_ResurgenceProto();
            proto.GameLevelId = ms.ReadInt();
            proto.Grade = (byte)ms.ReadByte();
            return proto;
        }
    }
}

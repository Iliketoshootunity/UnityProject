using System.Collections;
using System.Collections.Generic;

public class GameLevel_FailProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_FailProto;
        }
    }

    public int GameLevelId;

    public byte Grade;

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(GameLevelId);
            ms.WriteByte(Grade);
            return ms.ToArray();
        }
    }

    public static GameLevel_FailProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_FailProto proto = new GameLevel_FailProto();
            proto.GameLevelId = ms.ReadInt();
            proto.Grade = (byte)ms.ReadByte();
            return proto;
        }
    }
}

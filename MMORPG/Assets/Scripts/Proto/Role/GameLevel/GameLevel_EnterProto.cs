using System.Collections;
using System.Collections.Generic;


public class GameLevel_EnterProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.GameLevel_EnterProto;
        }
    }
    /// <summary>
    /// 游戏关卡Id
    /// </summary>
    public int GameLevelId;
    /// <summary>
    /// 关卡难度
    /// </summary>
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

    public static GameLevel_EnterProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            GameLevel_EnterProto proto = new GameLevel_EnterProto();
            proto.GameLevelId = ms.ReadInt();
            proto.Grade = (byte)ms.ReadByte();
            return proto;
        }
    }


}

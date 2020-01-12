using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色 登录服务器协议
/// </summary>
public class RoleOpration_LogOnGameServerProto : IProto
{
    public int AccoutID;

    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.LogOnGameServerProto;
        }
    }

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(AccoutID);
            return ms.ToArray();
        }
    }

    public static RoleOpration_LogOnGameServerProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOpration_LogOnGameServerProto proto = new RoleOpration_LogOnGameServerProto();
            proto.AccoutID = ms.ReadInt();
            return proto;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 服务器返回登录服务器结果——角色列表
/// </summary>
public class RoleOperation_LogOnGameServerReturnProto : IProto
{
    public class RoleItem
    {
        public int RoleId;

        public string NickName;

        public int Level;

        public byte RoleJob;
    }

    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.LogOnGameServerReturnProto;
        }
    }

    public int RoleCount;

    public List<RoleItem> Roles = new List<RoleItem>();

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(RoleCount);
            for (int i = 0; i < Roles.Count; i++)
            {
                ms.WriteInt(Roles[i].RoleId);
                ms.WriteUTF8String(Roles[i].NickName);
                ms.WriteInt(Roles[i].Level);
                ms.WriteByte(Roles[i].RoleJob);
            }
            return ms.ToArray();
        }
    }

    public static RoleOperation_LogOnGameServerReturnProto ToPoto(byte[] buffer)
    {
        RoleOperation_LogOnGameServerReturnProto proto = new RoleOperation_LogOnGameServerReturnProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.RoleCount = ms.ReadInt();
            for (int i = 0; i < proto.RoleCount; i++)
            {
                RoleItem item = new RoleItem();
                item.RoleId = ms.ReadInt();
                item.NickName = ms.ReadUTF8String();
                item.Level = ms.ReadInt();
                item.RoleJob = (byte)ms.ReadByte();
                proto.Roles.Add(item);
            }
            return proto;
        }
    }

}

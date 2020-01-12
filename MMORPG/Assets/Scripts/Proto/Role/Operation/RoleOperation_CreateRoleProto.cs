using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleOperation_CreateRoleProto : IProto
{
    public int JobID;

    public string NickName;
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.CreateRoleProto;
        }
    }

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(JobID);
            ms.WriteUTF8String(NickName);
            return ms.ToArray();
        }
    }

    public static RoleOperation_CreateRoleProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOperation_CreateRoleProto proto = new RoleOperation_CreateRoleProto();
            proto.JobID = ms.ReadInt();
            proto.NickName = ms.ReadUTF8String();
            return proto;
        }
    }
}

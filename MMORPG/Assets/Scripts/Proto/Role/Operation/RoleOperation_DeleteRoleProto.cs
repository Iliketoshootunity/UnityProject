using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleOperation_DeleteRoleProto : IProto {

    public int RoleID;


    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.DeleteRoleProto;
        }
    }

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(RoleID);
            return ms.ToArray();
        }
    }

    public static RoleOperation_DeleteRoleProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOperation_DeleteRoleProto proto = new RoleOperation_DeleteRoleProto();
            proto.RoleID = ms.ReadInt();
            return proto;
        }
    }
}

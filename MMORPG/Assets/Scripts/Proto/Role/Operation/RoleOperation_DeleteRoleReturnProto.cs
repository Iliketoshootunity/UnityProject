using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleOperation_DeleteRoleReturnProto : IProto {

    public bool IsSucess;

    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.DeleteRoleReturnProto;
        }
    }

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteBool(IsSucess);
            return ms.ToArray();
        }
    }

    public static RoleOperation_DeleteRoleReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOperation_DeleteRoleReturnProto proto = new RoleOperation_DeleteRoleReturnProto();
            proto.IsSucess = ms.ReadBool();
            return proto;
        }
    }
}

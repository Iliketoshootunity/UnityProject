using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleOperation_CreateRoleReturnProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.CreateRoleReturnProto;
        }
    }


    public bool IsSucess;

    public int MessageID = -1;


    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteBool(IsSucess);
            if (!IsSucess)
            {
                ms.WriteInt(MessageID);
            }
            return ms.ToArray();
        }
    }

    public static RoleOperation_CreateRoleReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOperation_CreateRoleReturnProto proto = new RoleOperation_CreateRoleReturnProto();
            proto.IsSucess = ms.ReadBool();
            if (!proto.IsSucess)
            {
                proto.MessageID = ms.ReadInt();
            }
            return proto;
        }
    }
}

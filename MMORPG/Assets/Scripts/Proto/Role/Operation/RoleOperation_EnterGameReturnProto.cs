using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleOperation_EnterGameReturnProto : IProto {

    public bool IsSucess;

    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.EnterGameReturnProto;
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

    public static RoleOperation_EnterGameReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOperation_EnterGameReturnProto proto = new RoleOperation_EnterGameReturnProto();
            proto.IsSucess = ms.ReadBool();
            return proto;
        }
    }
}

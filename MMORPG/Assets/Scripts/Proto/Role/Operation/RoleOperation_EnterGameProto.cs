using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleOperation_EnterGameProto : IProto {

    public int RoleID;


    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.EnterGameProto;
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

    public static RoleOperation_EnterGameProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOperation_EnterGameProto proto = new RoleOperation_EnterGameProto();
            proto.RoleID = ms.ReadInt();
            return proto;
        }
    }
}

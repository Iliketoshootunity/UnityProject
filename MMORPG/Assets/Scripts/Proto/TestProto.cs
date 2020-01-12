using System.Collections;
using System.Collections.Generic;


public struct TestProto : IProto
{
    public ushort ProtoCode
    {
        get
        {
            return (ushort)ProtoCodeDef.TestProto;
        }
    }

    public int Id;

    public string Name;

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(Id);
            ms.WriteUTF8String(Name);
            return ms.ToArray();

        }
    }

    public static TestProto GetProto(byte[] buffer)
    {
        TestProto proto = new TestProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.Id = ms.ReadInt();
            proto.Name = ms.ReadUTF8String();
        }
        return proto;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 从服务器上返回所选择的角色信息
/// </summary>
public class RoleOperation_SelectRoleInfoReturnProto : IProto
{
    public bool IsSucess;
    public int RoleId;
    public string RoleNickName;
    public int Level;
    public int JobId;
    public int Money;
    public int Gold;
    public int Exp;
    public int MaxHP;
    public int CurrentHP;
    public int MaxMP;
    public int CurrentMP;
    public int Attack;
    public int Defense;
    public int Hit;
    public int Dodge;
    public int Res;
    public int Cri;
    public int Fighting;
    public int LastSceneId;

    public int MessageId;
    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.SelectRoleInfoReturnProto;
        }
    }

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteBool(IsSucess);
            if (IsSucess)
            {
                ms.WriteUTF8String(RoleNickName);
                ms.WriteInt(Level);
                ms.WriteInt(RoleId);
                ms.WriteInt(JobId);
                ms.WriteInt(Money);
                ms.WriteInt(Gold);
                ms.WriteInt(Exp);
                ms.WriteInt(MaxHP);
                ms.WriteInt(CurrentHP);
                ms.WriteInt(MaxMP);
                ms.WriteInt(CurrentMP);
                ms.WriteInt(Attack);
                ms.WriteInt(Defense);
                ms.WriteInt(Hit);
                ms.WriteInt(Dodge);
                ms.WriteInt(Cri);
                ms.WriteInt(Fighting);
                ms.WriteInt(Res);
                ms.WriteInt(LastSceneId);
            }
            else
            {
                ms.WriteInt(MessageId);
            }
            return ms.ToArray();
        }
    }

    public static RoleOperation_SelectRoleInfoReturnProto ToProto(byte[] buffer)
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            RoleOperation_SelectRoleInfoReturnProto proto = new RoleOperation_SelectRoleInfoReturnProto();
            proto.IsSucess = ms.ReadBool();
            if (proto.IsSucess)
            {
                proto.RoleNickName = ms.ReadUTF8String();
                proto.Level = ms.ReadInt();
                proto.RoleId = ms.ReadInt();
                proto.JobId = ms.ReadInt();
                proto.Money = ms.ReadInt();
                proto.Gold = ms.ReadInt();
                proto.Exp = ms.ReadInt();
                proto.MaxHP = ms.ReadInt();
                proto.CurrentHP = ms.ReadInt();
                proto.MaxMP = ms.ReadInt();
                proto.CurrentMP = ms.ReadInt();
                proto.Attack = ms.ReadInt();
                proto.Defense = ms.ReadInt();
                proto.Hit = ms.ReadInt();
                proto.Dodge = ms.ReadInt();
                proto.Cri = ms.ReadInt();
                proto.Fighting = ms.ReadInt();
                proto.Res = ms.ReadInt();
                proto.LastSceneId = ms.ReadInt();
            }
            else
            {
                proto.MessageId = ms.ReadInt();
            }
            return proto;
        }
    }
}

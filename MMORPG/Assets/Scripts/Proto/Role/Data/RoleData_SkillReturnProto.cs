using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleData_SkillReturnProto : IProto
{
    public class SkillData
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public int SkillId;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int SKillLevel;

        /// <summary>
        /// 技能插槽
        /// </summary>
        public byte SlotsNO;
    }

    public ushort ProtoCode
    {
        get
        {
            return ProtoCodeDef.SkillReturnReturnProto;
        }
    }

    public int SkillCount;

    public List<SkillData> Skills = new List<SkillData>();

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(SkillCount);
            for (int i = 0; i < Skills.Count; i++)
            {
                ms.WriteInt(Skills[i].SkillId);
                ms.WriteInt(Skills[i].SKillLevel);
                ms.WriteByte(Skills[i].SlotsNO);
            }
            return ms.ToArray();
        }
    }

    public static RoleData_SkillReturnProto ToPoto(byte[] buffer)
    {
        RoleData_SkillReturnProto proto = new RoleData_SkillReturnProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.SkillCount = ms.ReadInt();
            for (int i = 0; i < proto.SkillCount; i++)
            {
                SkillData item = new SkillData();
                item.SkillId = ms.ReadInt();
                item.SKillLevel = ms.ReadInt();
                item.SlotsNO = (byte)ms.ReadByte();
                proto.Skills.Add(item);
            }
            return proto;
        }
    }

}

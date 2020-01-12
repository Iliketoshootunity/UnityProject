using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoSkill
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


    private float m_SkillCDTime = -1;

    public float SkillCDTime
    {
        get
        {
            if (m_SkillCDTime == -1)
            {
                SkillLevelEntity entity = SkillLevelDBModel.Instance.GetEnityBySkillIdAndSkillLevel(SkillId, SKillLevel);
                if (entity != null)
                {
                    m_SkillCDTime = entity.SkillCDTime;
                }

            }
            return m_SkillCDTime;
        }
    }

    /// <summary>
    /// 消耗的MP
    /// </summary>
    private float m_SpendMP = -1;

    public float SpendMP
    {
        get
        {
            if (m_SpendMP == -1)
            {
                SkillLevelEntity entity = SkillLevelDBModel.Instance.GetEnityBySkillIdAndSkillLevel(SkillId, SKillLevel);
                if (entity != null)
                {
                    m_SpendMP = entity.SpendMP;
                }
            }
            return m_SpendMP;
        }
    }
    /// <summary>
    /// 技能冷却结束时间
    /// </summary>
    public float SkillCDEndTime;



}

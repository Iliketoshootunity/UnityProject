using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoSkill
{

    /// <summary>
    /// ����ID
    /// </summary>
    public int SkillId;

    /// <summary>
    /// ���ܵȼ�
    /// </summary>
    public int SKillLevel;

    /// <summary>
    /// ���ܲ��
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
    /// ���ĵ�MP
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
    /// ������ȴ����ʱ��
    /// </summary>
    public float SkillCDEndTime;



}

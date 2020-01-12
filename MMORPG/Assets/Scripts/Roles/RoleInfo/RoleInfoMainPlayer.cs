using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoMainPlayer : RoleInfoBase
{
    /// <summary>
    /// 职业ID
    /// </summary>
    public int JobId;
    /// <summary>
    /// 金钱
    /// </summary>
    public int Money;
    /// <summary>
    /// 金币
    /// </summary>
    public int Gold;



    public RoleInfoMainPlayer()
    {

    }

    /// <summary>
    /// 加载角色信息
    /// </summary>
    /// <param name="proto"></param>
    public void LoadRoleInfo(RoleOperation_SelectRoleInfoReturnProto proto)
    {
        RoleId = proto.RoleId;
        RoleNickName = proto.RoleNickName;
        Level = proto.Level;
        JobId = proto.JobId;
        Money = proto.Money;
        Gold = proto.Gold;
        Exp = proto.Exp;
        MaxHP = proto.MaxHP;
        CurrentHP = proto.CurrentHP;
        MaxMP = proto.MaxMP;
        CurrentMP = proto.CurrentMP;
        Attack = proto.Attack;
        Defense = proto.Defense;
        Hit = proto.Hit;
        Dodge = proto.Dodge;
        Res = proto.Res;
        Cri = proto.Cri;
        Fighting = proto.Fighting;
    }

    /// <summary>
    /// 加载技能信息
    /// </summary>
    /// <param name="proto"></param>
    public void LoadSkill(RoleData_SkillReturnProto proto)
    {
        List<RoleData_SkillReturnProto.SkillData> lst = proto.Skills;
        for (int i = 0; i < lst.Count; i++)
        {
            RoleInfoSkill skillInfo = new RoleInfoSkill();
            skillInfo.SkillId = lst[i].SkillId;
            skillInfo.SKillLevel = lst[i].SKillLevel;
            skillInfo.SlotsNO = lst[i].SlotsNO;
            SkillList.Add(skillInfo);
        }
    }

    /// <summary>
    /// 加载物理攻击
    /// </summary>
    /// <param name="jobId"></param>
    public void LoadPhyAttack(int jobId)
    {
        PhyAttackList = JobDBModel.Instance.GetPhyAttackIDList(jobId);
    }
    /// <summary>
    /// 获取节能等级
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public int GetSkillLevel(int skillId)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].SkillId == skillId)
            {
                return SkillList[i].SKillLevel;
            }
        }
        return 1;
    }

    /// <summary>
    /// 设置技能冷却结束时间
    /// </summary>
    /// <param name="skillId"></param>
    public void SetSkillCDEndTime(int skillId)
    {
        if (SkillList.Count > 0)
        {
            for (int i = 0; i < SkillList.Count; i++)
            {
                if (SkillList[i].SkillId == skillId)
                {
                    SkillList[i].SkillCDEndTime = Time.time + SkillList[i].SkillCDTime;
                    return;
                }
            }
        }

    }
    /// <summary>
    /// 获取可以使用的技能
    /// </summary>
    /// <returns></returns>
    public int GetCanUseSkill()
    {
        if (SkillList.Count > 0)
        {
            for (int i = 0; i < SkillList.Count; i++)
            {
                if (Time.time > SkillList[i].SkillCDEndTime && CurrentMP > SkillList[i].SpendMP)
                {
                    return SkillList[i].SkillId;
                }
            }
        }
        return 0;
    }


}

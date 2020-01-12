using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻击类
/// </summary>
[System.Serializable]
public class RoleAttack
{
    /// <summary>
    /// 物理攻击表
    /// </summary>
    public List<RoleAttackInfo> PhyAttackList;

    /// <summary>
    /// 技能攻击表
    /// </summary>
    public List<RoleAttackInfo> SkillAttackList;

    /// <summary>
    /// 攻击的敌人列表
    /// </summary>
    private List<RoleCtrl> m_EnemyList;
    private List<Collider> m_SearchEnemyList;
    private RoleFSMMgr m_RoleFSMMgr;
    private RoleCtrl m_RoleCtrl;
    private RoleStateAttack m_StateAttak;
    /// <summary>
    /// 后续技能ID
    /// </summary>
    private int m_FollowSkillId;
    /// <summary>
    /// 后续技能ID
    /// </summary>
    public int FollowSkillId { get { return m_FollowSkillId; } }

    public bool IsAutoFight;
    public void Init(RoleFSMMgr fsm, RoleCtrl roleCtrl)
    {
        m_RoleFSMMgr = fsm;
        m_EnemyList = new List<RoleCtrl>();
        m_SearchEnemyList = new List<Collider>();
        m_RoleCtrl = roleCtrl;
    }

    #region 测试用
    public RoleAttackInfo GetAttackInfoByIndex(AttackType attackType, int index)
    {
        if (attackType == AttackType.PhyAttack)
        {
            return PhyAttackList.Find((x) => x.Index == index);
        }
        else
        {
            return SkillAttackList.Find((x) => x.Index == index);
        }

    }
    public void ToAttackByIndex(AttackType attackType, int index)
    {

        if (m_RoleFSMMgr == null) return;
        if (m_StateAttak == null)
        {
            m_StateAttak = (RoleStateAttack)m_RoleFSMMgr.GetState(RoleState.Attack);
            if (m_StateAttak == null) return;
        }
        m_RoleFSMMgr.CurRoleCtrl.CurAttackInfo = m_RoleFSMMgr.CurRoleCtrl.Attack.GetAttackInfoByIndex(attackType, index);
        m_StateAttak.AnimatorCondition = (attackType == AttackType.PhyAttack ? ToAnimatorCondition.ToPhyAttack.ToString() : ToAnimatorCondition.ToSkillAttack.ToString());
        m_StateAttak.AnimatorConditionValue = index;
        KeyValuePair<string, int> stateValue = GetAnimatorName(attackType, index);
        m_StateAttak.CurAnimatorName = stateValue.Key;
        m_StateAttak.CurrentStateValue = stateValue.Value;
        //必须能进去
        m_RoleFSMMgr.ChangeState(RoleState.Attack);
    }
    #endregion
    public bool ToAttackBySkillId(AttackType attackType, int skillId)
    {
        if (m_RoleFSMMgr == null || m_RoleCtrl.IsRigibody)
        {
            if (attackType == AttackType.SkillAttack)
            {
                m_FollowSkillId = skillId;
            }
            return false;
        }
        m_FollowSkillId = -1;
        #region 
        //只有主角和怪参与数值计算
        if (m_RoleCtrl.CurRoleType == RoleType.MainPlayer || m_RoleCtrl.CurRoleType == RoleType.Monster)
        {
            SkillEntity skillEntity = SkillDBModel.Instance.Get(skillId);
            if (skillEntity == null) return false;
            int skillLevel = 1;
            if (m_RoleCtrl.CurRoleType == RoleType.MainPlayer)
            {
                skillLevel = ((RoleInfoMainPlayer)m_RoleCtrl.CurRoleInfo).GetSkillLevel(skillId);
            }
            SkillLevelEntity skillLevelEnity = SkillLevelDBModel.Instance.GetEnityBySkillIdAndSkillLevel(skillId, skillLevel);
            //主角
            if (m_RoleCtrl.CurRoleType == RoleType.MainPlayer)
            {
                if (skillLevelEnity.SpendMP > m_RoleCtrl.CurRoleInfo.CurrentMP)
                {
                    return false;
                }
                m_RoleCtrl.CurRoleInfo.CurrentMP -= skillLevelEnity.SpendMP;
                if (m_RoleCtrl.CurRoleInfo.CurrentMP < 0)
                {
                    m_RoleCtrl.CurRoleInfo.CurrentMP = 0;
                }
                if (m_RoleCtrl.OnMPChange != null)
                {
                    m_RoleCtrl.OnMPChange(ValueChangeType.Reduce);
                }
            }
            //暂时 只有玩家才寻找敌人
            m_EnemyList.Clear();
            if (m_RoleCtrl.CurRoleType == RoleType.MainPlayer)
            {
                int attackTargetCount = skillEntity.AttackTargetCount;
                if (attackTargetCount == 1)
                {
                    //单体攻击
                    if (m_RoleCtrl.LockEnemy != null)
                    {
                        //有锁定敌人
                        m_EnemyList.Add(m_RoleCtrl.LockEnemy);
                    }
                    else
                    {
                        //没有锁定敌人
                        SearchAndSortEnemys(skillEntity.AreaAttackRadius);
                        bool isFind = false;
                        for (int i = 0; i < m_SearchEnemyList.Count; i++)
                        {
                            RoleCtrl lockEnemy = m_SearchEnemyList[i].GetComponent<RoleCtrl>();
                            if (lockEnemy.CurRoleType == RoleType.MainPlayer || lockEnemy.FSM.CurrentRoleStateEnum == RoleState.Die) continue;
                            m_EnemyList.Add(lockEnemy);
                            m_RoleCtrl.LockEnemy = lockEnemy;
                            break;
                        }
                        if (!isFind)
                        {
                            //找不到敌人
                            return false;
                        }

                    }
                }
                else
                {
                    //群体攻击
                    int needCount = attackTargetCount;
                    if (m_RoleCtrl.LockEnemy != null)
                    {
                        //有锁定敌人
                        m_EnemyList.Add(m_RoleCtrl.LockEnemy);
                        needCount--;
                        SearchAndSortEnemys(skillEntity.AreaAttackRadius);
                        //循环加入搜索到的敌人
                        for (int i = 0; i < m_SearchEnemyList.Count; i++)
                        {
                            RoleCtrl lockEnemy = m_SearchEnemyList[i].GetComponent<RoleCtrl>();
                            if (lockEnemy.CurRoleInfo.RoleId == m_RoleCtrl.LockEnemy.CurRoleInfo.RoleId || lockEnemy.CurRoleType == RoleType.MainPlayer || lockEnemy.FSM.CurrentRoleStateEnum == RoleState.Die) continue;
                            if (i + 1 > needCount) break;
                            m_EnemyList.Add(lockEnemy);
                        }

                    }
                    else
                    {
                        //没有锁定敌人
                        SearchAndSortEnemys(skillEntity.AreaAttackRadius);
                        if (m_SearchEnemyList.Count > 0)
                        {
                            for (int i = 0; i < m_SearchEnemyList.Count; i++)
                            {
                                if (i + 1 > needCount) break;
                                RoleCtrl lockEnemy = m_SearchEnemyList[i].GetComponent<RoleCtrl>();
                                if (lockEnemy.CurRoleType == RoleType.MainPlayer || lockEnemy.FSM.CurrentRoleStateEnum == RoleState.Die) continue;
                                if (i == 0)
                                {
                                    m_RoleCtrl.LockEnemy = lockEnemy;
                                }
                                m_EnemyList.Add(lockEnemy);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                }

            }
            else
            {
                m_EnemyList.Add(m_RoleCtrl.LockEnemy);
            }

            for (int i = 0; i < m_EnemyList.Count; i++)
            {
                RoleTransferAttackInfo t = CalculationHurtValue(m_EnemyList[i], skillEntity, skillLevelEnity);
                if (t == null) return false;
                m_EnemyList[i].ToHurt(t);
            }
        }


        #endregion

        #region 动画相关
        if (m_StateAttak == null)
        {
            m_StateAttak = (RoleStateAttack)m_RoleFSMMgr.GetState(RoleState.Attack);
            if (m_StateAttak == null) return false;
        }
        m_RoleFSMMgr.CurRoleCtrl.CurAttackInfo = GetAttackInfoBySkillId(attackType, skillId);
        m_StateAttak.AnimatorCondition = (attackType == AttackType.PhyAttack ? ToAnimatorCondition.ToPhyAttack.ToString() : ToAnimatorCondition.ToSkillAttack.ToString());
        m_StateAttak.AnimatorConditionValue = m_RoleFSMMgr.CurRoleCtrl.CurAttackInfo.Index;
        KeyValuePair<string, int> stateValue = GetAnimatorName(attackType, m_RoleFSMMgr.CurRoleCtrl.CurAttackInfo.Index);
        m_StateAttak.CurAnimatorName = stateValue.Key;
        m_StateAttak.CurrentStateValue = stateValue.Value;
        m_RoleFSMMgr.ChangeState(RoleState.Attack);
        #endregion
        return true;

    }

    private RoleTransferAttackInfo CalculationHurtValue(RoleCtrl enemy, SkillEntity skillEntity, SkillLevelEntity skillLevelEntity)
    {
        if (enemy == null || skillEntity == null || skillLevelEntity == null) return null;
        RoleTransferAttackInfo t = new RoleTransferAttackInfo();
        t.AttackRoleID = m_RoleCtrl.CurRoleInfo.RoleId;
        t.AttackRolePos = m_RoleCtrl.transform.position;
        t.BeAttaclRoleID = enemy.CurRoleInfo.RoleId;
        t.SkillId = skillEntity.Id;
        t.SkillLevel = skillLevelEntity.Level;
        t.IsAbnormal = skillEntity.AbnormalState == 1;

        //计算伤害
        //1.攻击数值=攻击方的综合战斗力*（技能的伤害倍率*0.01f）
        float attackValue = m_RoleCtrl.CurRoleInfo.Fighting * (skillLevelEntity.HurtValueRate * 0.01f);
        //2.基础伤害=g攻击数值*攻击数值/（攻击数值+被攻击方的防御）
        float baseHurt = attackValue * attackValue / (attackValue * enemy.CurRoleInfo.Defense);
        //3.暴击概率=0.05f+(攻击方的暴击/（攻击方的暴击+防御方的抗性）)*0.1f
        float cri = 0.05f + (m_RoleCtrl.CurRoleInfo.Cri / (m_RoleCtrl.CurRoleInfo.Cri / (float)enemy.CurRoleInfo.Res)) * 0.1f;
        cri = cri > 0.5f ? 0.5f : cri;
        //4.是否暴击=0-1随机数<=随机数
        bool isCri = Random.Range(0f, 1f) <= cri;
        //5.暴击伤害倍率=有暴击？1.5f:1f
        float criHurt = isCri ? 1.5f : 1f;
        //6.随机数 0.9f-1.1f
        float random = Random.Range(0.9f, 1.1f);
        //7.最终伤害 =基础伤害*暴击伤害倍率*随机数
        int hurtValue = (int)(baseHurt * criHurt * random);
        hurtValue = hurtValue < 1 ? 1 : hurtValue;
        t.IsCri = isCri;
        t.HurtValue = hurtValue;
        return t;
    }

    /// <summary>
    /// 寻找敌人
    /// </summary>
    /// <param name="range"></param>
    private void SearchAndSortEnemys(float range)
    {
        m_SearchEnemyList.Clear();
        Collider[] cols = Physics.OverlapSphere(m_RoleCtrl.transform.position, range, 1 << LayerMask.NameToLayer("Role"));
        if (cols != null && cols.Length > 0)
        {
            //找到敌人
            for (int i = 0; i < cols.Length; i++)
            {
                m_SearchEnemyList.Add(cols[i]);
            }
            m_SearchEnemyList.Sort((Collider c1, Collider c2) =>
            {
                if (Vector3.Distance(m_RoleCtrl.transform.position, c1.transform.position) < Vector3.Distance(m_RoleCtrl.transform.position, c2.transform.position))
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
        }
    }

    /// <summary>
    /// 获取攻击信息
    /// </summary>
    /// <param name="attackType"></param>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public RoleAttackInfo GetAttackInfoBySkillId(AttackType attackType, int skillId)
    {
        if (attackType == AttackType.PhyAttack)
        {
            return PhyAttackList.Find((x) => x.SkillId == skillId);
        }
        else
        {
            return SkillAttackList.Find((x) => x.SkillId == skillId);
        }
    }

    private Dictionary<string, int> m_dic;
    public KeyValuePair<string, int> GetAnimatorName(AttackType attackType, int index)
    {
        if (m_dic == null)
        {
            m_dic = new Dictionary<string, int>();
            m_dic["PhyAttack1"] = 11;
            m_dic["PhyAttack2"] = 12;
            m_dic["PhyAttack3"] = 13;
            m_dic["SkillAttack1"] = 14;
            m_dic["SkillAttack2"] = 15;
            m_dic["SkillAttack3"] = 16;
            m_dic["SkillAttack4"] = 17;
            m_dic["SkillAttack5"] = 18;
            m_dic["SkillAttack6"] = 19;
        }
        string key = ((attackType == AttackType.PhyAttack ? "PhyAttack" : "SkillAttack") + index.ToString());
        KeyValuePair<string, int> ret = new KeyValuePair<string, int>(key, m_dic[key]);
        return ret;
    }
}

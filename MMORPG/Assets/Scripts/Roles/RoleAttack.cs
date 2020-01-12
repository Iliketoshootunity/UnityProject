using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ������
/// </summary>
[System.Serializable]
public class RoleAttack
{
    /// <summary>
    /// ��������
    /// </summary>
    public List<RoleAttackInfo> PhyAttackList;

    /// <summary>
    /// ���ܹ�����
    /// </summary>
    public List<RoleAttackInfo> SkillAttackList;

    /// <summary>
    /// �����ĵ����б�
    /// </summary>
    private List<RoleCtrl> m_EnemyList;
    private List<Collider> m_SearchEnemyList;
    private RoleFSMMgr m_RoleFSMMgr;
    private RoleCtrl m_RoleCtrl;
    private RoleStateAttack m_StateAttak;
    /// <summary>
    /// ��������ID
    /// </summary>
    private int m_FollowSkillId;
    /// <summary>
    /// ��������ID
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

    #region ������
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
        //�����ܽ�ȥ
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
        //ֻ�����Ǻ͹ֲ�����ֵ����
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
            //����
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
            //��ʱ ֻ����Ҳ�Ѱ�ҵ���
            m_EnemyList.Clear();
            if (m_RoleCtrl.CurRoleType == RoleType.MainPlayer)
            {
                int attackTargetCount = skillEntity.AttackTargetCount;
                if (attackTargetCount == 1)
                {
                    //���幥��
                    if (m_RoleCtrl.LockEnemy != null)
                    {
                        //����������
                        m_EnemyList.Add(m_RoleCtrl.LockEnemy);
                    }
                    else
                    {
                        //û����������
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
                            //�Ҳ�������
                            return false;
                        }

                    }
                }
                else
                {
                    //Ⱥ�幥��
                    int needCount = attackTargetCount;
                    if (m_RoleCtrl.LockEnemy != null)
                    {
                        //����������
                        m_EnemyList.Add(m_RoleCtrl.LockEnemy);
                        needCount--;
                        SearchAndSortEnemys(skillEntity.AreaAttackRadius);
                        //ѭ�������������ĵ���
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
                        //û����������
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

        #region �������
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

        //�����˺�
        //1.������ֵ=���������ۺ�ս����*�����ܵ��˺�����*0.01f��
        float attackValue = m_RoleCtrl.CurRoleInfo.Fighting * (skillLevelEntity.HurtValueRate * 0.01f);
        //2.�����˺�=g������ֵ*������ֵ/��������ֵ+���������ķ�����
        float baseHurt = attackValue * attackValue / (attackValue * enemy.CurRoleInfo.Defense);
        //3.��������=0.05f+(�������ı���/���������ı���+�������Ŀ��ԣ�)*0.1f
        float cri = 0.05f + (m_RoleCtrl.CurRoleInfo.Cri / (m_RoleCtrl.CurRoleInfo.Cri / (float)enemy.CurRoleInfo.Res)) * 0.1f;
        cri = cri > 0.5f ? 0.5f : cri;
        //4.�Ƿ񱩻�=0-1�����<=�����
        bool isCri = Random.Range(0f, 1f) <= cri;
        //5.�����˺�����=�б�����1.5f:1f
        float criHurt = isCri ? 1.5f : 1f;
        //6.����� 0.9f-1.1f
        float random = Random.Range(0.9f, 1.1f);
        //7.�����˺� =�����˺�*�����˺�����*�����
        int hurtValue = (int)(baseHurt * criHurt * random);
        hurtValue = hurtValue < 1 ? 1 : hurtValue;
        t.IsCri = isCri;
        t.HurtValue = hurtValue;
        return t;
    }

    /// <summary>
    /// Ѱ�ҵ���
    /// </summary>
    /// <param name="range"></param>
    private void SearchAndSortEnemys(float range)
    {
        m_SearchEnemyList.Clear();
        Collider[] cols = Physics.OverlapSphere(m_RoleCtrl.transform.position, range, 1 << LayerMask.NameToLayer("Role"));
        if (cols != null && cols.Length > 0)
        {
            //�ҵ�����
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
    /// ��ȡ������Ϣ
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

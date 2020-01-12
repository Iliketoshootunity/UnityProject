using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleMainPlayerCityAI : IRoleAI
{
    public RoleCtrl CurRoleCtrl { get; set; }
    public RoleMainPlayerCityAI(RoleCtrl roleCtrl)
    {
        CurRoleCtrl = roleCtrl;
    }
    private int m_PhyAttackIndex = 0;
    private int m_AttackId = 0;
    private AttackType attckType;


    public void DoAI()
    {
        if (CurRoleCtrl.FSM.CurrentRoleStateEnum == RoleState.Die) return;
        if (CurRoleCtrl.Attack.IsAutoFight)
        {
            AutoFightState();
        }
        else
        {
            NormalState();
        }

    }

    /// <summary>
    /// 战斗
    /// </summary>
    private void NormalState()
    {
        if (CurRoleCtrl.LockEnemy != null)
        {
            if (CurRoleCtrl.LockEnemy.FSM.CurrentRoleStateEnum == RoleState.Die)
            {
                CurRoleCtrl.LockEnemy = null;
                return;
            }

            if (CurRoleCtrl.FSM.CurrentRoleStateEnum == RoleState.Idle)
            {
                if (CurRoleCtrl.Attack.FollowSkillId > 0)
                {
                    //模拟点击按钮
                    PlayerCtrl.Instance.ClickSkillButton(CurRoleCtrl.Attack.FollowSkillId);
                }
                else
                {
                    //确定使用的物理攻击ID
                    m_AttackId = CurRoleCtrl.CurRoleInfo.PhyAttackList[m_PhyAttackIndex];
                    //和目标的距离
                    float distance = Vector3.Distance(CurRoleCtrl.transform.position, CurRoleCtrl.LockEnemy.transform.position);
                    if (distance > CurRoleCtrl.AttackRange)
                    {
                        //距离过大，则追击
                        Vector3 m_MoveTarget = GameUtil.GetTargetSectorPoint(CurRoleCtrl.transform.position, CurRoleCtrl.LockEnemy.transform.position, Random.Range(0.9f, 1.1f) * CurRoleCtrl.AttackRange);
                        RaycastHit hitinfo1;
                        Ray ray1 = new Ray(new Vector3(m_MoveTarget.x, m_MoveTarget.y + 1000, m_MoveTarget.z), -Vector3.up);
                        if (Physics.Raycast(ray1, out hitinfo1, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
                        {
                            return;
                        }
                        CurRoleCtrl.MoveTo(m_MoveTarget);
                        return;
                    }
                    else
                    {
                        //否则攻击
                        CurRoleCtrl.ToAttack(AttackType.PhyAttack, m_AttackId);
                        m_PhyAttackIndex++;
                        if (m_PhyAttackIndex >= CurRoleCtrl.CurRoleInfo.PhyAttackList.Length)
                        {
                            m_PhyAttackIndex = 0;
                        }
                    }
                }
                CurRoleCtrl.ToAttack(attckType, m_AttackId);
                return;
            }
        }
    }

    private List<Collider> m_SearchPlayerList = new List<Collider>();
    /// <summary>
    /// 自动战斗
    /// </summary>
    private void AutoFightState()
    {

        if (CurRoleCtrl.IsRigibody) return;
        if (!GameLevelSceneCtrl.Instance.CurRegionHasMonster)   //当前区域没有怪了
        {
            //进入下一个区域
            if (GameLevelSceneCtrl.Instance.LastRegion)//如果是最后一个区域的怪
            {
                return;
            }
            else
            {
                Vector3 nextRegionPos = GameLevelSceneCtrl.Instance.NextRegionPlyaerPos;
                CurRoleCtrl.MoveTo(nextRegionPos);
            }

        }
        else    // 当前区域有怪
        {
            if(CurRoleCtrl.FSM.CurrentRoleStateEnum == RoleState.Idle)
            {
                //找怪打怪
                if (CurRoleCtrl.LockEnemy == null)  //当前没有锁定敌人
                {
                    //搜索敌人
                    m_SearchPlayerList.Clear();
                    Collider[] cols = Physics.OverlapSphere(CurRoleCtrl.transform.position, 1000, 1 << LayerMask.NameToLayer("Role"));
                    if (cols != null && cols.Length > 0)
                    {
                        //找到敌人
                        for (int i = 0; i < cols.Length; i++)
                        {
                            RoleCtrl localEnemy = cols[i].GetComponent<RoleCtrl>();
                            if (localEnemy.CurRoleType == RoleType.Monster && localEnemy.CurRoleInfo.CurrentHP > 0)
                            {
                                m_SearchPlayerList.Add(cols[i]);
                            }
                        }
                    }
                    if (m_SearchPlayerList.Count > 0)
                    {
                        m_SearchPlayerList.Sort((Collider c1, Collider c2) =>
                        {
                            if (Vector3.Distance(CurRoleCtrl.transform.position, c1.transform.position) < Vector3.Distance(CurRoleCtrl.transform.position, c2.transform.position))
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        });
                        CurRoleCtrl.LockEnemy = m_SearchPlayerList[0].GetComponent<RoleCtrl>();
                    }
                    else
                    {
                        CurRoleCtrl.LockEnemy = null;
                    }


                }
                else   //当前有锁定敌人
                {
                    if (CurRoleCtrl.LockEnemy.FSM.CurrentRoleStateEnum == RoleState.Die)
                    {
                        CurRoleCtrl.LockEnemy = null;
                        return;
                    }
                    //定义要使用的id和技能类型
                    int skillId = 0;
                    AttackType attackType = AttackType.PhyAttack;
                    //首先检测有没有可使用的技能
                    int canUseSkillId = ((RoleInfoMainPlayer)CurRoleCtrl.CurRoleInfo).GetCanUseSkill();
                    if (canUseSkillId > 0)   //如果有可使用的技能
                    {
                        //设置技能攻击
                        skillId = canUseSkillId;
                        attackType = AttackType.SkillAttack;
                    }
                    else
                    {
                        //设置物理攻击
                        skillId = CurRoleCtrl.CurRoleInfo.PhyAttackList[m_PhyAttackIndex]; ;
                        attackType = AttackType.PhyAttack;
                        m_PhyAttackIndex++;
                        if (m_PhyAttackIndex >= CurRoleCtrl.CurRoleInfo.PhyAttackList.Length)
                        {
                            m_PhyAttackIndex = 0;
                        }

                    }
                    SkillEntity skillEntity = SkillDBModel.Instance.Get(skillId);
                    if (skillEntity == null)
                    {
                        return;
                    }
                    float attackRange = skillEntity.AttackRange;
                    //攻击范围之内

                    if (Vector3.Distance(CurRoleCtrl.LockEnemy.transform.position, CurRoleCtrl.transform.position) < attackRange)
                    {
                        if (attackType == AttackType.PhyAttack)
                        {
                            CurRoleCtrl.ToAttack(attackType, skillId);
                        }
                        else
                        {
                            PlayerCtrl.Instance.ClickSkillButton(skillId);
                        }

                    }
                    else //追击
                    {
                        if (CurRoleCtrl.FSM.CurrentRoleStateEnum == RoleState.Idle)
                        {
                            Vector3 m_MoveTarget = GameUtil.GetTargetSectorPoint(CurRoleCtrl.transform.position, CurRoleCtrl.LockEnemy.transform.position, Random.Range(0.9f, 1.1f) * attackRange);
                            RaycastHit hitinfo1;
                            Ray ray1 = new Ray(new Vector3(m_MoveTarget.x, m_MoveTarget.y + 1000, m_MoveTarget.z), -Vector3.up);
                            if (Physics.Raycast(ray1, out hitinfo1, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
                            {
                                return;
                            }
                            CurRoleCtrl.MoveTo(m_MoveTarget);
                        }

                    }
                }
            }

        }

    }
}

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
    /// ս��
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
                    //ģ������ť
                    PlayerCtrl.Instance.ClickSkillButton(CurRoleCtrl.Attack.FollowSkillId);
                }
                else
                {
                    //ȷ��ʹ�õ�������ID
                    m_AttackId = CurRoleCtrl.CurRoleInfo.PhyAttackList[m_PhyAttackIndex];
                    //��Ŀ��ľ���
                    float distance = Vector3.Distance(CurRoleCtrl.transform.position, CurRoleCtrl.LockEnemy.transform.position);
                    if (distance > CurRoleCtrl.AttackRange)
                    {
                        //���������׷��
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
                        //���򹥻�
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
    /// �Զ�ս��
    /// </summary>
    private void AutoFightState()
    {

        if (CurRoleCtrl.IsRigibody) return;
        if (!GameLevelSceneCtrl.Instance.CurRegionHasMonster)   //��ǰ����û�й���
        {
            //������һ������
            if (GameLevelSceneCtrl.Instance.LastRegion)//��������һ������Ĺ�
            {
                return;
            }
            else
            {
                Vector3 nextRegionPos = GameLevelSceneCtrl.Instance.NextRegionPlyaerPos;
                CurRoleCtrl.MoveTo(nextRegionPos);
            }

        }
        else    // ��ǰ�����й�
        {
            if(CurRoleCtrl.FSM.CurrentRoleStateEnum == RoleState.Idle)
            {
                //�ҹִ��
                if (CurRoleCtrl.LockEnemy == null)  //��ǰû����������
                {
                    //��������
                    m_SearchPlayerList.Clear();
                    Collider[] cols = Physics.OverlapSphere(CurRoleCtrl.transform.position, 1000, 1 << LayerMask.NameToLayer("Role"));
                    if (cols != null && cols.Length > 0)
                    {
                        //�ҵ�����
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
                else   //��ǰ����������
                {
                    if (CurRoleCtrl.LockEnemy.FSM.CurrentRoleStateEnum == RoleState.Die)
                    {
                        CurRoleCtrl.LockEnemy = null;
                        return;
                    }
                    //����Ҫʹ�õ�id�ͼ�������
                    int skillId = 0;
                    AttackType attackType = AttackType.PhyAttack;
                    //���ȼ����û�п�ʹ�õļ���
                    int canUseSkillId = ((RoleInfoMainPlayer)CurRoleCtrl.CurRoleInfo).GetCanUseSkill();
                    if (canUseSkillId > 0)   //����п�ʹ�õļ���
                    {
                        //���ü��ܹ���
                        skillId = canUseSkillId;
                        attackType = AttackType.SkillAttack;
                    }
                    else
                    {
                        //����������
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
                    //������Χ֮��

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
                    else //׷��
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

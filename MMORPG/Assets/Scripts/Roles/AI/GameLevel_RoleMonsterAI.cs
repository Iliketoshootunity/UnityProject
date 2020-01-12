using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel_RoleMonsterAI : IRoleAI
{
    public RoleCtrl CurRoleCtrl { get; set; }
    public RoleInfoMonster RoleInfo;
    private float patrolTime;
    private float seekTime;
    private float m_NextAttackTime;
    private AttackType m_AttackType;
    private Vector3 m_MoveTarget;

    /// <summary>
    /// 下次思考时间
    /// </summary>
    private float m_NextThikTime;
    /// <summary>
    /// 下次发呆时间
    /// </summary>
    private float m_NextDazeTime;
    /// <summary>
    /// 是否发呆
    /// </summary>
    private bool m_IsDaze;
    public GameLevel_RoleMonsterAI(RoleCtrl roleCtrl, RoleInfoMonster info)
    {
        CurRoleCtrl = roleCtrl;
        RoleInfo = info;
    }
    public void DoAI()
    {
        if (CurRoleCtrl.FSM.CurrentRoleStateEnum == RoleState.Die)
        {
            return;
        }
        //如果没有锁定敌人
        if (CurRoleCtrl.LockEnemy == null)
        {
            if (CurRoleCtrl.FSM.CurrentRoleStateEnum == RoleState.Idle)
            {
                if (Time.time > patrolTime)
                {
                    //执行巡逻     
                    m_MoveTarget = new Vector3(Random.Range(-CurRoleCtrl.PatrolRange, CurRoleCtrl.PatrolRange), 0, Random.Range(-CurRoleCtrl.PatrolRange, CurRoleCtrl.PatrolRange)) + new Vector3(CurRoleCtrl.BornPoint.x, CurRoleCtrl.transform.position.y, CurRoleCtrl.BornPoint.z);
                    RaycastHit hitinfo1;
                    Ray ray1 = new Ray(new Vector3(m_MoveTarget.x, m_MoveTarget.y + 1000, m_MoveTarget.z), -Vector3.up);
                    if (Physics.Raycast(ray1, out hitinfo1, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
                    {
                        return;
                    }
                    CurRoleCtrl.MoveTo(m_MoveTarget);
                    patrolTime = Time.time + Random.Range(3, 5);
                }

            }
            //寻找敌人
            SearchAndSortPlayers(CurRoleCtrl.ViewRange);
            if (m_SearchPlayerList.Count > 0)
            {
                CurRoleCtrl.LockEnemy = m_SearchPlayerList[0].GetComponent<RoleCtrl>();
                m_NextAttackTime = Time.time + Random.Range(0, 1) + RoleInfo.SpriteEntity.Attack_Interval;
                return;
            }
            else
            {
                CurRoleCtrl.LockEnemy = null;
            }
        }
        //如果有锁定敌人
        else
        {
            if (CurRoleCtrl.LockEnemy.FSM.CurrentRoleStateEnum == RoleState.Die)
            {
                CurRoleCtrl.LockEnemy = null;
                return;
            }
            //做了3 -3.5秒其他事  准备休息一下 
            if (Time.time > m_NextThikTime + Random.Range(3, 3.5f))
            {
                CurRoleCtrl.ToIdle(IdleType.IdleFight);
                m_NextThikTime = Time.time;
                m_NextDazeTime = Time.time;
                m_IsDaze = true;
            }
            //休息中
            if (m_IsDaze)
            {
                if (Time.time > m_NextDazeTime + Random.Range(1, 1.5f))
                {
                    m_IsDaze = false;
                }
                else
                {
                    return;
                }
            }
            if (CurRoleCtrl.FSM.CurrentRoleStateEnum != RoleState.Idle) return;
            //查过视野范围之外
            if (Vector3.Distance(CurRoleCtrl.LockEnemy.transform.position, CurRoleCtrl.transform.position) > CurRoleCtrl.ViewRange)
            {
                CurRoleCtrl.LockEnemy = null;
                return;
            }
            //视野范围之内
            else
            {
                //获取技能ID
                bool isUsePhyAttack = RoleInfo.SpriteEntity.PhysicalAttackRate > Random.Range(0f, 100f);
                int skillId = 0;
                if (isUsePhyAttack)
                {
                    m_AttackType = AttackType.PhyAttack;
                    skillId = RoleInfo.SpriteEntity.GetPhyAttackList[Random.Range(0, RoleInfo.SpriteEntity.GetPhyAttackList.Length)];
                }
                else
                {
                    m_AttackType = AttackType.SkillAttack;
                    skillId = RoleInfo.SpriteEntity.GetSkillAttackList[Random.Range(0, RoleInfo.SpriteEntity.GetSkillAttackList.Length)];
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
                    if (Time.time > m_NextAttackTime)
                    {
                        m_NextAttackTime = Time.time + Random.Range(0, 1) + RoleInfo.SpriteEntity.Attack_Interval;
                        CurRoleCtrl.ToAttack(m_AttackType, skillId);
                        return;
                    }

                }
                else
                {
                    if (Time.time > seekTime)
                    {
                        m_MoveTarget = GameUtil.GetTargetSectorPoint(CurRoleCtrl.transform.position, CurRoleCtrl.LockEnemy.transform.position, Random.Range(0.9f, 1.1f) * attackRange);
                        RaycastHit hitinfo1;
                        Ray ray1 = new Ray(new Vector3(m_MoveTarget.x, m_MoveTarget.y + 1000, m_MoveTarget.z), -Vector3.up);
                        if (Physics.Raycast(ray1, out hitinfo1, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
                        {
                            return;
                        }
                        CurRoleCtrl.MoveTo(m_MoveTarget);
                        seekTime = Time.time + Random.Range(3, 5);
                    }
                }

            }
        }
    }

    private List<Collider> m_SearchPlayerList = new List<Collider>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="range"></param>
    private void SearchAndSortPlayers(float range)
    {
        m_SearchPlayerList.Clear();
        Collider[] cols = Physics.OverlapSphere(CurRoleCtrl.transform.position, range, 1 << LayerMask.NameToLayer("Role"));
        if (cols != null && cols.Length > 0)
        {
            //找到敌人
            for (int i = 0; i < cols.Length; i++)
            {
                RoleCtrl localEnemy = cols[i].GetComponent<RoleCtrl>();
                if ((localEnemy.CurRoleType == RoleType.MainPlayer || localEnemy.CurRoleType == RoleType.OtherPlayer)
                    && localEnemy.FSM.CurrentRoleStateEnum != RoleState.Die)
                {
                    m_SearchPlayerList.Add(cols[i]);
                }
            }
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
        }
    }
}

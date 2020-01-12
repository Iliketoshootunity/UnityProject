using UnityEngine;
using System.Collections;
using System;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(FunnelModifier))]
public class RoleCtrl : MonoBehaviour
{

    public bool IsDebug;

    /// <summary>
    /// 移动目标点
    /// </summary>
    [HideInInspector]
    public Vector3 TargetPos;
    /// <summary>
    /// 出生点
    /// </summary>
    [HideInInspector]
    public Vector3 BornPoint;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float MoveSpeed = 10;
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float RotateSpeed = 10;
    /// <summary>
    /// 视野范围
    /// </summary>
    public float ViewRange = 8;
    /// <summary>
    ///巡逻范围
    /// </summary>
    public float PatrolRange = 10;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange = 1.5f;

    /// <summary>
    /// 血条挂点
    /// </summary>
    [SerializeField]
    private Transform headBarPos;

    /// <summary>
    /// 角色控制器
    /// </summary>
    [HideInInspector]
    public CharacterController CurCharacterController;
    /// <summary>
    /// 动画状态机
    /// </summary>
    [HideInInspector]
    public Animator Animator;
    /// <summary>
    /// 角色类型
    /// </summary>
    [HideInInspector]
    public RoleType CurRoleType;
    /// <summary>
    /// 角色信息
    /// </summary>
    public RoleInfoBase CurRoleInfo;
    /// <summary>
    /// 角色AI
    /// </summary>
    public IRoleAI CurRoleAI;
    /// <summary>
    /// 锁定的成员
    /// </summary>
    [HideInInspector]
    public RoleCtrl LockEnemy;
    /// <summary>
    /// 角色状态机
    /// </summary>
    public RoleFSMMgr FSM;
    /// <summary>
    /// 橘色血条
    /// </summary>
    private RoleHeadBar headBar;
    /// <summary>
    /// 受伤委托
    /// </summary>
    public Action OnRoleHurt;
    /// <summary>
    /// 死亡委托
    /// </summary>
    public Action<RoleCtrl> OnRoleDie;
    /// <summary>
    /// 删除物体委托
    /// </summary>
    public Action<Transform> OnRoleDestory;

    public delegate void OnValueChange(ValueChangeType type);
    public OnValueChange OnHPChange;
    public OnValueChange OnMPChange;

    private bool isInit;


    //-------------------寻路相关----------------------------------//

    /// <summary>
    /// AstartSeeker
    /// </summary>
    [HideInInspector]
    public Seeker AstartSeeker;

    /// <summary>
    /// A*路径
    /// </summary>
    [HideInInspector]
    public ABPath AstartPath;

    /// <summary>
    ///  A*路径 索引
    /// </summary>
    [HideInInspector]
    public int AstartWayPointIndex;

    //------------------------战斗相关----------------------------------//
    /// <summary>
    /// 攻击逻辑
    /// </summary>
    public RoleAttack Attack;
    /// <summary>
    /// 受伤逻辑
    /// </summary>
    private RoleHurt m_Hurt;
    /// <summary>
    /// 当前的攻击信息
    /// </summary>
    public RoleAttackInfo CurAttackInfo;
    /// <summary>
    /// 是否僵值
    /// </summary>
    [HideInInspector]
    public bool IsRigibody;
    /// <summary>
    /// 角色控制器初始化
    /// </summary>
    /// <param name="roleType"></param>
    /// <param name="roleInfo"></param>
    /// <param name="ai"></param>
    public void Init(RoleType roleType, RoleInfoBase roleInfo, IRoleAI ai)
    {
        CurRoleType = roleType;
        CurRoleInfo = roleInfo;
        CurRoleAI = ai;
        if (CurCharacterController == null)
        {
            CurCharacterController = GetComponent<CharacterController>();
        }
        CurCharacterController.enabled = true;
        isInit = true;
    }

    #region Mono 流程


    // Use this for initialization
    void Start()
    {

        CurCharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        AstartSeeker = GetComponent<Seeker>();
        if (CurRoleType == RoleType.MainPlayer)
        {
            CameraCtrl.Instance.Init();
        }
        FSM = new RoleFSMMgr(this, OnDestroyCallBack, OnDieCallBack);
        m_Hurt = new RoleHurt(FSM);
        m_Hurt.OnHurt = OnHurt;
        Attack.Init(FSM, this);
    }




    // Update is called once per frame
    void Update()
    {
        FSM.OnUpdate();
        if (CurRoleAI == null) return;
        CurRoleAI.DoAI();
        if (isInit)
        {
            isInit = false;
            if (CurRoleType == RoleType.Monster)
            {
                ToIdle(IdleType.IdleFight);
            }
            else
            {
                ToIdle(IdleType.IdleNormal);
            }
        }
        if (CurRoleType == RoleType.MainPlayer)
        {
            CameraAutoFollow();
        }
        AutoSmallMap();
    }

    private void OnDestroy()
    {
        if (headBar != null)
        {
            Destroy(headBar.gameObject);
        }
    }

    #endregion
    #region 状态切换
    /// <summary>
    /// 切穿到Idle
    /// </summary>
    public void ToIdle(IdleType idleType)
    {
        FSM.NextIdleType = idleType;
        FSM.ChangeState(RoleState.Idle);
    }
    /// <summary>
    /// 切换到庆祝状态
    /// </summary>
    public void ToSelect()
    {
        FSM.ChangeState(RoleState.Select);
    }
    /// <summary>
    /// 测试代码
    /// </summary>
    public void ToRun()
    {
        FSM.ChangeState(RoleState.Run);
    }
    /// <summary>
    /// 移动到目标点
    /// </summary>
    /// <param name="targetPos"></param>
    public void MoveTo(Vector3 targetPos)
    {
        if (FSM.CurrentRoleStateEnum == RoleState.Die) return;
        if (targetPos == Vector3.zero) return;
        AstartPath = null;
        FSM.ChangeState(RoleState.Run);
        Path path = AstartSeeker.StartPath(transform.position, targetPos, (p) =>
        {
            AstartPath = (ABPath)p;
            AstartWayPointIndex = 1;
            if (AstartPath != null)
            {
                if (Vector3.Distance(AstartPath.endPoint, new Vector3(AstartPath.originalEndPoint.x, AstartPath.endPoint.y, AstartPath.originalEndPoint.z)) > 0.5f)
                {
                    Debug.Log("目标点有障碍物，过不去");
                    AstartPath = null;
                    FSM.ChangeState(RoleState.Idle);
                    return;
                }
            }
        });


    }

    public bool ToAttack(AttackType type, int skillId)
    {
        if (FSM == null) return false;
        if (FSM.CurrentRoleStateEnum == RoleState.Die) return false;
        bool isSucess = Attack.ToAttackBySkillId(type, skillId);
        if (!isSucess) return false;
        if (!string.IsNullOrEmpty(CurAttackInfo.EffectName))
        {
            Transform effect = EffectMgr.Instance.PlayEffect(CurAttackInfo.EffectName);
            effect.position = transform.position;
            effect.rotation = transform.rotation;
        }

        return true;
    }
    #region 测试使用 切换到攻击状态
    /// <summary>
    /// 切换到攻击 ,测试使用
    /// </summary>
    public void ToAttackByIndex(AttackType type, int index)
    {

        Attack.ToAttackByIndex(type, index);
        Transform effect = EffectMgr.Instance.PlayEffect(CurAttackInfo.EffectName.ToLower());
        effect.position = transform.position;
        effect.rotation = transform.rotation;
    }
    #endregion
    /// <summary>
    /// 切换到受伤画面
    /// </summary>
    public void ToHurt(RoleTransferAttackInfo attackInfo)
    {
        StartCoroutine(m_Hurt.ToHurt(attackInfo));
    }


    /// <summary>
    /// 切换到死亡
    /// </summary>
    public void ToDie()
    {
        FSM.ChangeState(RoleState.Die);
    }
    #endregion

    /// <summary>
    /// 复活
    /// </summary>
    public void ToResurrection()
    {
        CurRoleInfo.CurrentHP = CurRoleInfo.MaxHP;
        CurRoleInfo.CurrentMP = CurRoleInfo.MaxMP;
        CurCharacterController.enabled = true;
        ToIdle(IdleType.IdleFight);
        if (OnHPChange != null)
        {
            OnHPChange(ValueChangeType.Add);
        }
        if (OnMPChange != null)
        {
            OnMPChange(ValueChangeType.Add);
        }
        LockEnemy = null;
    }
    /// <summary>
    /// 出生
    /// </summary>
    public void Born(Vector3 bornPoint)
    {
        InitRoleHeadBar();
        transform.position = bornPoint;
        BornPoint = bornPoint;
    }

    private void OnDieCallBack()
    {
        if (OnRoleDie != null)
        {
            OnRoleDie(this);
        }
    }
    private void OnDestroyCallBack()
    {
        if (OnRoleDestory != null)
        {
            OnRoleDestory(transform);
        }
        CurCharacterController.enabled = false;
        if (headBar != null)
        {
            Destroy(headBar.gameObject);
        }
    }

    private void OnHurt()
    {
        if (headBar != null)
        {
            headBar.SetHpSlider(hpSliderValue: CurRoleInfo.CurrentHP / (float)CurRoleInfo.MaxHP);
        }
        if (OnRoleHurt != null)
        {
            OnRoleHurt();
        }
        if (OnHPChange != null)
        {
            OnHPChange(ValueChangeType.Reduce);
        }
    }

    /// <summary>
    /// 初始化角色的血条
    /// </summary>
    public void InitRoleHeadBar()
    {
        if (RoleHeadBarCtrl.Instance == null) return;
        GameObject go = ResourcesMrg.Instance.Load(ResourcesMrg.ResourceType.UIOther, "RoleHeadBar", isCache: true);
        go.transform.parent = RoleHeadBarCtrl.Instance.transform;
        go.transform.localScale = Vector3.one;
        headBar = go.GetComponent<RoleHeadBar>();
        headBar.Init(headBarPos.gameObject, CurRoleInfo.RoleNickName, CurRoleType == RoleType.MainPlayer ? false : true, hpSliderValue: CurRoleInfo.CurrentHP / (float)CurRoleInfo.MaxHP);
    }


    #region 摄像控制

    /// <summary>
    /// 摄像机自动跟随
    /// </summary>
    private void CameraAutoFollow()
    {
        if (CameraCtrl.Instance == null) return;
        CameraCtrl.Instance.transform.position = gameObject.transform.position;
        CameraCtrl.Instance.AutoLookAt(gameObject.transform.position);

    }

    #endregion

    private void AutoSmallMap()
    {
        if (SmallMapHelper.Instance == null || UISmallMapView.Instance == null) return;
        SmallMapHelper.Instance.transform.position = transform.position;

        UISmallMapView.Instance.transform.localPosition = new Vector3(SmallMapHelper.Instance.transform.localPosition.x * -512, SmallMapHelper.Instance.transform.localPosition.z * -512, 1);
    }

    private void OnDrawGizmos()
    {
        if (Global.Instance == null) return;
        if (Global.Instance.IsDeug)
        {


        }
        if (CurAttackInfo != null)
        {
            Gizmos.DrawWireSphere(transform.position, CurAttackInfo.AttackRange);
        }
        if (TargetPos != Vector3.zero)
        {
            Gizmos.DrawWireSphere(TargetPos, 1);
        }
        
    }


}

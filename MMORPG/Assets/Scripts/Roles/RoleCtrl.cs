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
    /// �ƶ�Ŀ���
    /// </summary>
    [HideInInspector]
    public Vector3 TargetPos;
    /// <summary>
    /// ������
    /// </summary>
    [HideInInspector]
    public Vector3 BornPoint;
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public float MoveSpeed = 10;
    /// <summary>
    /// ��ת�ٶ�
    /// </summary>
    public float RotateSpeed = 10;
    /// <summary>
    /// ��Ұ��Χ
    /// </summary>
    public float ViewRange = 8;
    /// <summary>
    ///Ѳ�߷�Χ
    /// </summary>
    public float PatrolRange = 10;
    /// <summary>
    /// ������Χ
    /// </summary>
    public float AttackRange = 1.5f;

    /// <summary>
    /// Ѫ���ҵ�
    /// </summary>
    [SerializeField]
    private Transform headBarPos;

    /// <summary>
    /// ��ɫ������
    /// </summary>
    [HideInInspector]
    public CharacterController CurCharacterController;
    /// <summary>
    /// ����״̬��
    /// </summary>
    [HideInInspector]
    public Animator Animator;
    /// <summary>
    /// ��ɫ����
    /// </summary>
    [HideInInspector]
    public RoleType CurRoleType;
    /// <summary>
    /// ��ɫ��Ϣ
    /// </summary>
    public RoleInfoBase CurRoleInfo;
    /// <summary>
    /// ��ɫAI
    /// </summary>
    public IRoleAI CurRoleAI;
    /// <summary>
    /// �����ĳ�Ա
    /// </summary>
    [HideInInspector]
    public RoleCtrl LockEnemy;
    /// <summary>
    /// ��ɫ״̬��
    /// </summary>
    public RoleFSMMgr FSM;
    /// <summary>
    /// ��ɫѪ��
    /// </summary>
    private RoleHeadBar headBar;
    /// <summary>
    /// ����ί��
    /// </summary>
    public Action OnRoleHurt;
    /// <summary>
    /// ����ί��
    /// </summary>
    public Action<RoleCtrl> OnRoleDie;
    /// <summary>
    /// ɾ������ί��
    /// </summary>
    public Action<Transform> OnRoleDestory;

    public delegate void OnValueChange(ValueChangeType type);
    public OnValueChange OnHPChange;
    public OnValueChange OnMPChange;

    private bool isInit;


    //-------------------Ѱ·���----------------------------------//

    /// <summary>
    /// AstartSeeker
    /// </summary>
    [HideInInspector]
    public Seeker AstartSeeker;

    /// <summary>
    /// A*·��
    /// </summary>
    [HideInInspector]
    public ABPath AstartPath;

    /// <summary>
    ///  A*·�� ����
    /// </summary>
    [HideInInspector]
    public int AstartWayPointIndex;

    //------------------------ս�����----------------------------------//
    /// <summary>
    /// �����߼�
    /// </summary>
    public RoleAttack Attack;
    /// <summary>
    /// �����߼�
    /// </summary>
    private RoleHurt m_Hurt;
    /// <summary>
    /// ��ǰ�Ĺ�����Ϣ
    /// </summary>
    public RoleAttackInfo CurAttackInfo;
    /// <summary>
    /// �Ƿ�ֵ
    /// </summary>
    [HideInInspector]
    public bool IsRigibody;
    /// <summary>
    /// ��ɫ��������ʼ��
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

    #region Mono ����


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
    #region ״̬�л�
    /// <summary>
    /// �д���Idle
    /// </summary>
    public void ToIdle(IdleType idleType)
    {
        FSM.NextIdleType = idleType;
        FSM.ChangeState(RoleState.Idle);
    }
    /// <summary>
    /// �л�����ף״̬
    /// </summary>
    public void ToSelect()
    {
        FSM.ChangeState(RoleState.Select);
    }
    /// <summary>
    /// ���Դ���
    /// </summary>
    public void ToRun()
    {
        FSM.ChangeState(RoleState.Run);
    }
    /// <summary>
    /// �ƶ���Ŀ���
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
                    Debug.Log("Ŀ������ϰ������ȥ");
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
    #region ����ʹ�� �л�������״̬
    /// <summary>
    /// �л������� ,����ʹ��
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
    /// �л������˻���
    /// </summary>
    public void ToHurt(RoleTransferAttackInfo attackInfo)
    {
        StartCoroutine(m_Hurt.ToHurt(attackInfo));
    }


    /// <summary>
    /// �л�������
    /// </summary>
    public void ToDie()
    {
        FSM.ChangeState(RoleState.Die);
    }
    #endregion

    /// <summary>
    /// ����
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
    /// ����
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
    /// ��ʼ����ɫ��Ѫ��
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


    #region �������

    /// <summary>
    /// ������Զ�����
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

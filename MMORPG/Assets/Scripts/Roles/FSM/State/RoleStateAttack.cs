using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoleStateAttack : RoleStateAbstract
{

    /// <summary>
    /// 动画过度条件
    /// </summary>
    public string AnimatorCondition;

    /// <summary>
    /// 旧的动画过度条件
    /// </summary>
    private string m_OldAnimatorCondition;
    /// <summary>
    /// 动画过度条件的值
    /// </summary>
    public int AnimatorConditionValue;

    /// <summary>
    /// 动画的名字
    /// </summary>
    public string CurAnimatorName;

    /// <summary>
    /// 当前状态的值
    /// </summary>
    public int CurrentStateValue;

    public RoleStateAttack(RoleFSMMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {
        m_OldAnimatorCondition = AnimatorCondition;
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(AnimatorCondition, AnimatorConditionValue);
        if (CurRoleFSMMgr.CurRoleCtrl.LockEnemy != null)
        {
            CurRoleFSMMgr.CurRoleCtrl.transform.LookAt(CurRoleFSMMgr.CurRoleCtrl.LockEnemy.transform.position);
        }
        CurRoleFSMMgr.CurRoleCtrl.IsRigibody = true;

    }
    public override void OnLevel()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(m_OldAnimatorCondition, 0);
        CurRoleFSMMgr.CurRoleCtrl.IsRigibody = false;
    }

    public override void OnUpdate()
    {
        CurAniamatorStateInfo = CurRoleFSMMgr.CurRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurAniamatorStateInfo.IsName(CurAnimatorName))
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), CurrentStateValue);
            if (CheckPlayOneOver())
            {
                Debug.Log("Attack Over");
                CurRoleFSMMgr.CurRoleCtrl.ToIdle(CurRoleFSMMgr.CurIdleType);
                CurRoleFSMMgr.CurRoleCtrl.IsRigibody = false;
            }
        }
        else
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
        }

    }
}

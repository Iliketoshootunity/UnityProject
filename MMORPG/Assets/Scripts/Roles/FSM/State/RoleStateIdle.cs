using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleStateIdle : RoleStateAbstract
{

    public RoleStateIdle(RoleFSMMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {
        if (CurRoleFSMMgr.CurIdleType == IdleType.IdleNormal)
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), true);
        }
        else
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.TpIdleFight.ToString(), true);
        }


    }
    public override void OnLevel()
    {
        if (CurRoleFSMMgr.CurIdleType == IdleType.IdleNormal)
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), false);
        }
        else
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.TpIdleFight.ToString(), false);
        }
    }

    public override void OnUpdate()
    {
        CurAniamatorStateInfo = CurRoleFSMMgr.CurRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurRoleFSMMgr.CurIdleType == IdleType.IdleNormal)
        {
            if (CurAniamatorStateInfo.IsName(RoleAniamtorName.Idle_Normal.ToString()))
            {
                CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), (int)RoleState.Idle);
            }
            else
            {
                CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
            }
        }
        if (CurRoleFSMMgr.CurIdleType == IdleType.IdleFight)
        {
            if (CurAniamatorStateInfo.IsName(RoleAniamtorName.Idle_Fight.ToString()))
            {
                CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), (int)RoleState.Idle);
            }
            else
            {
                CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
            }
        }
    }
}

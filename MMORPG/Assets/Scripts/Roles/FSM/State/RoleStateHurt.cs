using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleStateHurt : RoleStateAbstract
{
    public RoleStateHurt(RoleFSMMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToHurt.ToString(), true);
    }
    public override void OnLevel()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToHurt.ToString(), false);
    }

    public override void OnUpdate()
    {
        CurAniamatorStateInfo = CurRoleFSMMgr.CurRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurAniamatorStateInfo.IsName(RoleAniamtorName.Hurt.ToString()))
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), (int)CurRoleFSMMgr.CurrentRoleStateEnum);
            if (CurAniamatorStateInfo.normalizedTime > 1)
            {
                CurRoleFSMMgr.ChangeState(RoleState.Idle);
            }
        }
        else
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Çì×£×´Ì¬
/// </summary>
public class RoleStateSelect : RoleStateAbstract
{
    public RoleStateSelect(RoleFSMMgr mgr) : base(mgr)
    {

    }
    public override void OnEnter()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToSelect.ToString(), true);
    }

    public override void OnLevel()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToSelect.ToString(), false);
    }

    public override void OnUpdate()
    {
        CurAniamatorStateInfo = CurRoleFSMMgr.CurRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurAniamatorStateInfo.IsName(RoleAniamtorName.Select.ToString()))
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), (int)RoleState.Select);
            if(CheckPlayOneOver())
            {
                CurRoleFSMMgr.CurRoleCtrl.ToIdle(IdleType.IdleNormal);
            }
        }
        else
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
        }
    }
}

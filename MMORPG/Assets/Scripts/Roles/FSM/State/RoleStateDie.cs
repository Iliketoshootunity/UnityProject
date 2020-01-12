using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleStateDie : RoleStateAbstract
{
    public Action OnDestory;
    public Action OnDie;
    private float beginTime;
    private bool m_IsDestroy;
    public RoleStateDie(RoleFSMMgr mgr, Action onDestory, Action onDie) : base(mgr)
    {
        OnDestory = onDestory;
        OnDie = onDie;
    }


    public override void OnEnter()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDie.ToString(), true);
        if (OnDie != null)
        {
            OnDie();
        }
        beginTime = 0;
        m_IsDestroy = false;
    }
    public override void OnLevel()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDie.ToString(), false);
    }

    public override void OnUpdate()
    {

        if (!m_IsDestroy)
        {
            beginTime += Time.deltaTime;
            if (beginTime > 6)
            {
                if (OnDestory != null)
                {
                    OnDestory();
                    m_IsDestroy = true;
                    return;
                }
            }
        }

        CurAniamatorStateInfo = CurRoleFSMMgr.CurRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurAniamatorStateInfo.IsName(RoleAniamtorName.Die.ToString()))
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), (int)CurRoleFSMMgr.CurrentRoleStateEnum);
            if (CurAniamatorStateInfo.normalizedTime > 1)
            {
            }
        }
        else
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoleStateAbstract
{
    protected RoleFSMMgr CurRoleFSMMgr { get; private set; }
    protected AnimatorStateInfo CurAniamatorStateInfo;
    public RoleStateAbstract(RoleFSMMgr mgr)
    {
        CurRoleFSMMgr = mgr;
    }

    /// <summary>
    /// ����״̬��
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// ����״̬��
    /// </summary>
    public abstract void OnUpdate();
    /// <summary>
    /// �뿪״̬��
    /// </summary>
    public abstract void OnLevel();

    /// <summary>
    /// �ж϶����Ƿ��л�����
    /// </summary>
    /// <returns></returns>
    public bool CheckInTransition()
    {
        if (CurRoleFSMMgr.CurRoleCtrl.Animator.IsInTransition(0) == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// �жϵ�ǰ�����Ѿ�������һ��
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayOneOver()
    {
        if (CurAniamatorStateInfo.normalizedTime > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

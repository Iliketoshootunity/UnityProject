using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ܲ�״̬
/// </summary>
public class RoleStateRun : RoleStateAbstract
{

    public RoleStateRun(RoleFSMMgr mgr) : base(mgr)
    {

    }
    public override void OnEnter()
    {
        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToRun.ToString(), true);
    }
    public override void OnLevel()
    {

        CurRoleFSMMgr.CurRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToRun.ToString(), false);
    }

    public override void OnUpdate()
    {
        if (Global.Instance.IsDeug)
        {
            CurAniamatorStateInfo = CurRoleFSMMgr.CurRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
            if (CurAniamatorStateInfo.IsName(RoleAniamtorName.Run.ToString()))
            {
                CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), (int)CurRoleFSMMgr.CurrentRoleStateEnum);
            }
            else
            {
                CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
            }

            if (CurRoleFSMMgr.CurRoleCtrl.AstartPath == null)
            {

            }
            return;
        }
        //����Ϊ���Դ���
        CurAniamatorStateInfo = CurRoleFSMMgr.CurRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        //�п������л���Idle״̬ �ڼ� ���л��� Run ��ʱ�Ķ�����Idle �� Run ���ں��� �����Բ���ִ�������ж�
        if (CurAniamatorStateInfo.IsName(RoleAniamtorName.Run.ToString()))
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), (int)CurRoleFSMMgr.CurrentRoleStateEnum);
        }
        else
        {
            CurRoleFSMMgr.CurRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrentState.ToString(), 0);
        }

        if (CurRoleFSMMgr.CurRoleCtrl.CurCharacterController == null) return;

        if (CurRoleFSMMgr.CurRoleCtrl.AstartPath == null)
        {
            //CurRoleFSMMgr.CurRoleCtrl.ToIdle(CurRoleFSMMgr.CurIdleType);
            return;
        }
        if (CurRoleFSMMgr.CurRoleCtrl.AstartWayPointIndex >= CurRoleFSMMgr.CurRoleCtrl.AstartPath.vectorPath.Count)
        {
            CurRoleFSMMgr.CurRoleCtrl.ToIdle(CurRoleFSMMgr.CurIdleType);
            return;
        }

        if (!CurRoleFSMMgr.CurRoleCtrl.CurCharacterController.isGrounded)
        {
            CurRoleFSMMgr.CurRoleCtrl.CurCharacterController.Move(Vector3.up * -1000 * Time.deltaTime);
        }

        Vector3 wayPoint = CurRoleFSMMgr.CurRoleCtrl.AstartPath.vectorPath[CurRoleFSMMgr.CurRoleCtrl.AstartWayPointIndex];
        Vector3 tempTarget = new Vector3(wayPoint.x, CurRoleFSMMgr.CurRoleCtrl.transform.position.y, wayPoint.z);
        Vector3 dirction = tempTarget - CurRoleFSMMgr.CurRoleCtrl.transform.position;
        if (CurRoleFSMMgr.CurRoleCtrl.RotateSpeed <= 1)
        {
            CurRoleFSMMgr.CurRoleCtrl.RotateSpeed += 5f;
        }
        Quaternion targetRotate = Quaternion.LookRotation(dirction);
        CurRoleFSMMgr.CurRoleCtrl.transform.rotation = Quaternion.Lerp(CurRoleFSMMgr.CurRoleCtrl.transform.rotation, targetRotate, Time.deltaTime * CurRoleFSMMgr.CurRoleCtrl.RotateSpeed);
        CurRoleFSMMgr.CurRoleCtrl.CurCharacterController.Move(dirction.normalized * CurRoleFSMMgr.CurRoleCtrl.MoveSpeed * Time.deltaTime);
        if ((CurRoleFSMMgr.CurRoleCtrl.transform.position - tempTarget).magnitude <= 0.6f)
        {
            CurRoleFSMMgr.CurRoleCtrl.AstartWayPointIndex++;
        }


    }
}

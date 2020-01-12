using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleHurt
{
    private RoleFSMMgr m_RoleFSMMgr;
    public Action OnHurt;
    public RoleHurt(RoleFSMMgr fsm)
    {
        m_RoleFSMMgr = fsm;
    }

    public IEnumerator ToHurt(RoleTransferAttackInfo attackInfo)
    {
        if (m_RoleFSMMgr == null) yield break;
        SkillEntity skillEntity = SkillDBModel.Instance.Get(attackInfo.SkillId);
        SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.GetEnityBySkillIdAndSkillLevel(attackInfo.SkillId, attackInfo.SkillLevel);
        if (skillEntity == null || skillLevelEntity == null) yield break;
        yield return new WaitForSeconds(skillEntity.ShowHurtEffectDelaySecond);
        //减血
        m_RoleFSMMgr.CurRoleCtrl.CurRoleInfo.CurrentHP -= attackInfo.HurtValue;
        if (OnHurt != null)
        {
            OnHurt();
        }
        if (m_RoleFSMMgr.CurRoleCtrl.CurRoleInfo.CurrentHP < 0)
        {
            m_RoleFSMMgr.CurRoleCtrl.CurRoleInfo.CurrentHP = 0;
            m_RoleFSMMgr.CurRoleCtrl.ToDie();
            yield break;
        }
        //播放受伤特效
        int fontSize = 1;
        Color color = Color.red;
        if (attackInfo.IsCri)
        {
            fontSize = 2;
            color = Color.yellow;
        }
        UISceneCtrl.Instance.CurrentUIScene.HUD.NewText("- " + attackInfo.HurtValue.ToString(), m_RoleFSMMgr.CurRoleCtrl.transform, color, fontSize, 15f, -1f, 2.2f, UnityEngine.Random.Range(0, 2) == 1 ? bl_Guidance.LeftDown : bl_Guidance.RightDown);
        if (!m_RoleFSMMgr.CurRoleCtrl.IsRigibody)
        {
            m_RoleFSMMgr.ChangeState(RoleState.Hurt);
        }

    }

}

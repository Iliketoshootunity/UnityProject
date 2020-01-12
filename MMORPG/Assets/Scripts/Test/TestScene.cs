using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using LitJson;
using UnityEngine.SceneManagement;

public class TestScene : MonoBehaviour
{

    public RoleCtrl Ctrl;

    private void OnGUI()
    {
        float y = 0;
        if (GUI.Button(new Rect(new Vector2(0, y), new Vector2(100, 60)), "战斗待机"))
        {
            Ctrl.ToIdle(IdleType.IdleFight);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(0, y), new Vector2(100, 60)), "普通待机"))
        {
            Ctrl.ToIdle(IdleType.IdleNormal);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(0, y), new Vector2(100, 60)), "休闲待机"))
        {
            //Ctrl.ToIdle();
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(0, y), new Vector2(100, 60)), "跑"))
        {
            Ctrl.ToRun();
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(0, y), new Vector2(100, 60)), "庆祝动作"))
        {
            Ctrl.ToSelect();
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(0, y), new Vector2(100, 60)), "受伤"))
        {
            Ctrl.FSM.ChangeState(RoleState.Hurt);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(0, y), new Vector2(100, 60)), "死亡"))
        {
            Ctrl.FSM.ChangeState(RoleState.Die);
        }
        y = 0;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "物理攻击1"))
        {
            Ctrl.ToAttackByIndex(AttackType.PhyAttack, 1);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "物理攻击2"))
        {
            Ctrl.ToAttackByIndex(AttackType.PhyAttack, 2);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "物理攻击3"))
        {
            Ctrl.ToAttackByIndex(AttackType.PhyAttack, 3);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "技能攻击1"))
        {
            Ctrl.ToAttackByIndex(AttackType.SkillAttack, 1);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "技能攻击2"))
        {
            Ctrl.ToAttackByIndex(AttackType.SkillAttack, 2);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "技能攻击3"))
        {
            Ctrl.ToAttackByIndex(AttackType.SkillAttack, 3);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "技能攻击4"))
        {
            Ctrl.ToAttackByIndex(AttackType.SkillAttack, 4);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "技能攻击4"))
        {
            Ctrl.ToAttackByIndex(AttackType.SkillAttack, 5);
        }
        y = y + 70;
        if (GUI.Button(new Rect(new Vector2(110, y), new Vector2(100, 60)), "技能攻击4"))
        {
            Ctrl.ToAttackByIndex(AttackType.SkillAttack, 6);
        }

    }

}

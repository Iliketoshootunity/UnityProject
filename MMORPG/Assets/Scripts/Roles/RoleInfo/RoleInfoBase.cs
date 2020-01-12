using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoBase
{

    /// <summary>
    /// 角色ID
    /// </summary>
    public int RoleId;
    /// <summary>
    /// 昵称
    /// </summary>
    public string RoleNickName;
    /// <summary>
    /// 昵称
    /// </summary>
    public int Level;
    /// <summary>
    /// 经验
    /// </summary>
    public int Exp;
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// 当前生命值
    /// </summary>
    public int CurrentHP;
    /// <summary>
    /// 最大魔法值
    /// </summary>
    public int MaxMP;
    /// <summary>
    /// 当前魔法值
    /// </summary>
    public int CurrentMP;
    /// <summary>
    /// 攻击力
    /// </summary>
    public int Attack;
    /// <summary>
    /// 防御力
    /// </summary>
    public int Defense;
    /// <summary>
    /// 命中率
    /// </summary>
    public int Hit;
    /// <summary>
    /// 闪避率
    /// </summary>
    public int Dodge;
    /// <summary>
    /// 抗性
    /// </summary>
    public int Res;
    /// <summary>
    /// 暴击率
    /// </summary>
    public int Cri;
    /// <summary>
    /// 综合战斗力
    /// </summary>
    public int Fighting;

    /// <summary>
    /// 能使用的技能信息
    /// </summary>
    public List<RoleInfoSkill> SkillList = new List<RoleInfoSkill>();

    /// <summary>
    /// 能使用的普通攻击信息
    /// </summary>
    public int[] PhyAttackList;




}

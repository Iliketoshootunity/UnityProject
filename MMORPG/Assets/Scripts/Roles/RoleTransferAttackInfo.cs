using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻击信息传输类
/// </summary>
public class RoleTransferAttackInfo
{
    /// <summary>
    /// 攻击者ID
    /// </summary>
    public int AttackRoleID;
    /// <summary>
    /// 被攻击者id
    /// </summary>
    public int BeAttaclRoleID;

    /// <summary>
    /// 攻击者位置
    /// </summary>
    public Vector3 AttackRolePos;
    /// <summary>
    /// 攻击技能Id
    /// </summary>
    public int SkillId;
    /// <summary>
    /// 攻击技能编号
    /// </summary>
    public int SkillLevel;
    /// <summary>
    /// 是否附加异常状态
    /// </summary>
    public bool IsAbnormal;
    /// <summary>
    /// 是否暴击
    /// </summary>
    public bool IsCri;
    /// <summary>
    /// 伤害数值
    /// </summary>
    public int HurtValue;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻击信息
/// </summary>
[System.Serializable]
public class RoleAttackInfo
{

    /// <summary>
    /// 编号 和 动画状态机的条件编号一致
    /// </summary>
    public int Index;
    /// <summary>
    /// 
    /// </summary>
    public int SkillId;

    /// <summary>
    /// 特效名字
    /// </summary>
    public string EffectName;

    /// <summary>
    /// 特效存在时间
    /// </summary>
    public float EffectLifeTime;

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// 攻击延迟
    /// </summary>
    public float HurtDelay;

    /// <summary>
    /// 是否屏幕震动
    /// </summary>
    public bool IsCameraShake;

    /// <summary>
    /// 震动延迟
    /// </summary>
    public float CameraShakeDelay;
}

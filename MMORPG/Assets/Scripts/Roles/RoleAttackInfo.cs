using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ������Ϣ
/// </summary>
[System.Serializable]
public class RoleAttackInfo
{

    /// <summary>
    /// ��� �� ����״̬�����������һ��
    /// </summary>
    public int Index;
    /// <summary>
    /// 
    /// </summary>
    public int SkillId;

    /// <summary>
    /// ��Ч����
    /// </summary>
    public string EffectName;

    /// <summary>
    /// ��Ч����ʱ��
    /// </summary>
    public float EffectLifeTime;

    /// <summary>
    /// ������Χ
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// �����ӳ�
    /// </summary>
    public float HurtDelay;

    /// <summary>
    /// �Ƿ���Ļ��
    /// </summary>
    public bool IsCameraShake;

    /// <summary>
    /// ���ӳ�
    /// </summary>
    public float CameraShakeDelay;
}

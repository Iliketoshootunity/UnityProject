using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ������Ϣ������
/// </summary>
public class RoleTransferAttackInfo
{
    /// <summary>
    /// ������ID
    /// </summary>
    public int AttackRoleID;
    /// <summary>
    /// ��������id
    /// </summary>
    public int BeAttaclRoleID;

    /// <summary>
    /// ������λ��
    /// </summary>
    public Vector3 AttackRolePos;
    /// <summary>
    /// ��������Id
    /// </summary>
    public int SkillId;
    /// <summary>
    /// �������ܱ��
    /// </summary>
    public int SkillLevel;
    /// <summary>
    /// �Ƿ񸽼��쳣״̬
    /// </summary>
    public bool IsAbnormal;
    /// <summary>
    /// �Ƿ񱩻�
    /// </summary>
    public bool IsCri;
    /// <summary>
    /// �˺���ֵ
    /// </summary>
    public int HurtValue;
}

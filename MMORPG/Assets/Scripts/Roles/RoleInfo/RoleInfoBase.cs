using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoBase
{

    /// <summary>
    /// ��ɫID
    /// </summary>
    public int RoleId;
    /// <summary>
    /// �ǳ�
    /// </summary>
    public string RoleNickName;
    /// <summary>
    /// �ǳ�
    /// </summary>
    public int Level;
    /// <summary>
    /// ����
    /// </summary>
    public int Exp;
    /// <summary>
    /// �������ֵ
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    public int CurrentHP;
    /// <summary>
    /// ���ħ��ֵ
    /// </summary>
    public int MaxMP;
    /// <summary>
    /// ��ǰħ��ֵ
    /// </summary>
    public int CurrentMP;
    /// <summary>
    /// ������
    /// </summary>
    public int Attack;
    /// <summary>
    /// ������
    /// </summary>
    public int Defense;
    /// <summary>
    /// ������
    /// </summary>
    public int Hit;
    /// <summary>
    /// ������
    /// </summary>
    public int Dodge;
    /// <summary>
    /// ����
    /// </summary>
    public int Res;
    /// <summary>
    /// ������
    /// </summary>
    public int Cri;
    /// <summary>
    /// �ۺ�ս����
    /// </summary>
    public int Fighting;

    /// <summary>
    /// ��ʹ�õļ�����Ϣ
    /// </summary>
    public List<RoleInfoSkill> SkillList = new List<RoleInfoSkill>();

    /// <summary>
    /// ��ʹ�õ���ͨ������Ϣ
    /// </summary>
    public int[] PhyAttackList;




}

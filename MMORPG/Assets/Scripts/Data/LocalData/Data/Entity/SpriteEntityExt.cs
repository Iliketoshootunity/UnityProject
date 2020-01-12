using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpriteEntity
{

    /// <summary>
    /// 获取物理攻击
    /// </summary>
    public int[] GetPhyAttackList
    {
        get
        {
            if (string.IsNullOrEmpty(UsedPhyAttack)) return null;
            string[] arr = UsedPhyAttack.Split('_');
            int[] valueArr = new int[arr.Length];
            for (int i = 0; i < valueArr.Length; i++)
            {
                valueArr[i] = arr[i].ToInt();
            }
            return valueArr;
        }
    }

    /// <summary>
    /// 获取物理攻击
    /// </summary>
    public int[] GetSkillAttackList
    {
        get
        {
            if (string.IsNullOrEmpty(UsedSkillList)) return null;
            string[] arr = UsedSkillList.Split('_');
            int[] valueArr = new int[arr.Length];
            for (int i = 0; i < valueArr.Length; i++)
            {
                valueArr[i] = arr[i].ToInt();
            }
            return valueArr;
        }
    }

}

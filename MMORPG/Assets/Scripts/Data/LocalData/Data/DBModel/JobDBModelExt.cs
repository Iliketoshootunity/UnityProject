using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class JobDBModel
{
    public int[] GetPhyAttackIDList(int jobId)
    {
        JobEntity entity = Get(jobId);
        if (entity == null) return null;
        string str = entity.UsedPhyAttackIds;
        string[] strArr = str.Split(';');
        int[] ids = new int[strArr.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = strArr[i].ToInt();
        }
        return ids;
    }

}

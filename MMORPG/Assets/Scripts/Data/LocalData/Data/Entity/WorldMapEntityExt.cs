using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldMapEntity
{

    public Vector3 GetBornPoint
    {
        get
        {
            string[] arr = RoleBirthPos.Split('_');
            if (arr.Length < 3)
            {
                return Vector3.zero;
            }
            float x = 0, y = 0, z = 0;
            float.TryParse(arr[0], out x);
            float.TryParse(arr[1], out y);
            float.TryParse(arr[2], out z);
            return new Vector3(x, y, z);
        }
    }
    public float GetBornRotateY
    {
        get
        {
            string[] arr = RoleBirthPos.Split('_');
            if (arr.Length < 4)
            {
                return 0;
            }
            float y = 0;
            float.TryParse(arr[3], out y);
            return y;
        }
    }

    private List<WorldMapNpcData> npcDataList;
    public List<WorldMapNpcData> NPCDataList
    {
        get
        {
            if (npcDataList == null)
            {
                npcDataList = new List<WorldMapNpcData>();
                string[] npcArr = NPCList.Split('|');
                for (int i = 0; i < npcArr.Length; i++)
                {
                    string[] npcDataArr = npcArr[i].Split('_');
                    if (npcDataArr.Length < 6)
                    {
                        break;
                    }
                    WorldMapNpcData data = new WorldMapNpcData();
                    for (int j = 0; j < npcDataArr.Length; j++)
                    {
                   
                        int.TryParse(npcDataArr[0], out data.NPCId);
                        float x = 0, y = 0, z = 0, rotateY;
                        float.TryParse(npcDataArr[1], out x);
                        float.TryParse(npcDataArr[2], out y);
                        float.TryParse(npcDataArr[3], out z);
                        float.TryParse(npcDataArr[4], out rotateY);
                        data.NpcBornPoint = new Vector3(x, y, z);
                        data.NpCBornRotateY = rotateY;
                        data.Prologue = npcDataArr[5];      
                    }
                    npcDataList.Add(data);
                }
            }
            return npcDataList;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameLevelEntity
{

    private Vector2 m_LocalPoint = Vector2.zero;

    public Vector2 GetPosInMap()
    {
        if (!string.IsNullOrEmpty(PosInMap))
        {
            string[] strArr = PosInMap.Split('_');
            if (strArr.Length == 2)
            {
                float x = 0, y = 0;
                float.TryParse(strArr[0], out x);
                float.TryParse(strArr[1], out y);
                m_LocalPoint = new Vector2(x, y);
            }
        }
        return m_LocalPoint;
    }

}

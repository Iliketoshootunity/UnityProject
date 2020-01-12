using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameLevelGradeDBModel
{

    public GameLevelGradeEntity GetEnityByLevelIdAndGrade(int levelId, GameLevelGrade grade)
    {
        if (m_List != null && m_List.Count > 0)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                if (m_List[i].GameLevelId == levelId && m_List[i].EnumGrade == grade)
                {
                    return m_List[i];
                }
            }
        }
        return null;
    }

}

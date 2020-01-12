using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameLevelRegionDBModel
{

    private List<GameLevelRegionEntity> lst = new List<GameLevelRegionEntity>();

    public List<GameLevelRegionEntity> GetRegionListByGameLevelId(int id)
    {
        lst.Clear();
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == id)
            {
                lst.Add(m_List[i]);
            }
        }
        return lst;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameLevelDBModel
{

    private List<GameLevelEntity> m_NeedGameLevelList = new List<GameLevelEntity>();

    public List<GameLevelEntity> GetNeedEntityById(int chapterId)
    {
        if (m_List == null || m_List.Count == 0) return null;
        m_NeedGameLevelList.Clear();
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].ChapterID == chapterId)
            {
                m_NeedGameLevelList.Add(m_List[i]);
            }
        }
        return m_NeedGameLevelList;
    }


}

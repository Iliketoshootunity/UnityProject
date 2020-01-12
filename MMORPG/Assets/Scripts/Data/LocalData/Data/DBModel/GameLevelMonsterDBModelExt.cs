using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameLevelMonsterDBModel
{


    /// <summary>
    /// 获取本关卡下的怪的数量
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    public int GetLevelMonsterCount(int gameLevelId, GameLevelGrade grade)
    {
        int count = 0;
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade)
            {
                count += m_List[i].SpriteCount;
            }
        }
        return count;
    }

    /// <summary>
    /// 获取本关卡某区域的怪的数量
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <param name="regionId"></param>
    public int GetRegionMonsterCount(int gameLevelId, GameLevelGrade grade, int regionId)
    {
        int count = 0;
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade && m_List[i].RegionId == regionId)
            {
                count += m_List[i].SpriteCount;
            }
        }
        return count;
    }

    /// <summary>
    /// 获得本关卡的所有的怪的id
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <returns></returns>
    public int[] GetLevelAllMonsterId(int gameLevelId, GameLevelGrade grade)
    {
        List<int> lst = new List<int>();
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade)
            {
                if (!lst.Contains(m_List[i].SpriteId))
                {
                    lst.Add(m_List[i].SpriteId);
                }

            }
        }
        return lst.ToArray();
    }


    private List<GameLevelMonsterEntity> monstList = new List<GameLevelMonsterEntity>();

    /// <summary>
    /// 获得本关卡的某区域的所有的怪的信息
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <param name="regionId"></param>
    /// <returns></returns>
    public List<GameLevelMonsterEntity> GetRegionAllMonsterInfo(int gameLevelId, GameLevelGrade grade, int regionId)
    {
        monstList.Clear();
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade && m_List[i].RegionId == regionId)
            {

                monstList.Add(m_List[i]);
            }
        }
        return monstList;
    }

    public GameLevelMonsterEntity GetRegionMonsterInfo(int gameLevelId, GameLevelGrade grade, int regionId, int spriteId)
    {

        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade && m_List[i].RegionId == regionId && m_List[i].SpriteId == spriteId)
            {

                return m_List[i];
            }
        }
        return null;
    }

}

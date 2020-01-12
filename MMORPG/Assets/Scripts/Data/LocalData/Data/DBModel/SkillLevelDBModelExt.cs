using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SkillLevelDBModel
{
    public SkillLevelEntity GetEnityBySkillIdAndSkillLevel(int skillId, int skillLevel)
    {
        if (m_List != null && m_List.Count > 0)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                if (m_List[i].SkillId == skillId && m_List[i].Level == skillLevel)
                {
                    return m_List[i];
                }
            }
        }
        return null;
    }

}

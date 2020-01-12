using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCtrl : MonoBehaviour
{

    /// <summary>
    /// 橘色血条
    /// </summary>
    private NPCHeadBar headBar;

    /// <summary>
    /// 血条挂点
    /// </summary>
    [SerializeField]
    private Transform headBarPos;

    private WorldMapNpcData worldMapData;

    private NPCEntity entity;

    private string[] m_TalkArr;
    private float m_TalkShowTime;
    public void Init(WorldMapNpcData npcDate)
    {
        worldMapData = npcDate;
        entity = NPCDBModel.Instance.Get(npcDate.NPCId);
        InitNPCHeadBar();
        m_TalkArr = entity.Talk.Split('|');
    }

    private void Start()
    {
        m_TalkShowTime = Time.time;
    }
    private void Update()
    {
        if (Time.time > m_TalkShowTime)
        {
            m_TalkShowTime += 10;
            if (m_TalkArr.Length > 0)
            {
                headBar.InitTalk(m_TalkArr[Random.Range(0, m_TalkArr.Length)], 5);
            }

        }
    }
    /// <summary>
    /// 初始化角色的
    /// </summary>
    private void InitNPCHeadBar()
    {
        GameObject go = ResourcesMrg.Instance.Load(ResourcesMrg.ResourceType.UIOther, "NPCHeadBar", isCache: true);
        go.transform.parent = RoleHeadBarCtrl.Instance.transform;
        go.transform.localScale = Vector3.one;
        headBar = go.GetComponent<NPCHeadBar>();
        headBar.Init(headBarPos.gameObject, entity.Name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCtrlMgr : MonoBehaviour
{

    [SerializeField]
    private GameObject m_WorldMapCtrl;
    [SerializeField]
    private GameObject m_GameLevelCtrl;

    private Dictionary<SceneType, GameObject> m_Dic = new Dictionary<SceneType, GameObject>();

    private void Awake()
    {
        if (m_WorldMapCtrl != null)
        {
            m_Dic[SceneType.WorldMap] = m_WorldMapCtrl;
        }
        if (m_GameLevelCtrl != null)
        {
            m_Dic[SceneType.Gamelevel] = m_GameLevelCtrl;
        }
        foreach (var item in m_Dic)
        {
            if (item.Key == SceneMgr.Instance.CurrentType)
            {
                m_Dic[item.Key].SetActive(true);
            }
            else
            {
                m_Dic[item.Key].SetActive(false);
            }
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

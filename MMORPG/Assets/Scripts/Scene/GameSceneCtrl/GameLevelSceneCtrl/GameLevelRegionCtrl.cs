using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏关卡区域控制器
/// </summary>

public class GameLevelRegionCtrl : MonoBehaviour
{
    /// <summary>
    /// 区域ID
    /// </summary>
    public int RegionId;
    public GameObject RegionMaskObj;
    [SerializeField]
    private GameLevelDoorCtrl[] m_AllDoor;
    [SerializeField]
    private Transform m_RoleBornPos;
    [SerializeField]
    private Transform[] m_MonsterBornPos;

    public Transform RoleBornPos { get { return m_RoleBornPos; } }
    public Vector3 GetMonsterBornPos
    {
        get
        {
            if (m_MonsterBornPos.Length == 0) return Vector3.zero;
            Transform t = m_MonsterBornPos[Random.Range(0, m_MonsterBornPos.Length)];
            return t.TransformPoint(new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)));

        }
    }
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < m_AllDoor.Length; i++)
        {
            m_AllDoor[i].OwnerRegionId = RegionId;
        }
    }

    public GameLevelDoorCtrl GetNextRegionDoor(int regionIndex)
    {
        if (m_AllDoor != null && m_AllDoor.Length > 0)
        {
            for (int i = 0; i < m_AllDoor.Length; i++)
            {
                if (m_AllDoor[i].m_ConenctDoor.OwnerRegionId == regionIndex)
                {
                    return m_AllDoor[i];
                }
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.blue;
        if (m_RoleBornPos != null)
        {
            Gizmos.DrawSphere(m_RoleBornPos.position, 1f);
            Gizmos.DrawLine(m_RoleBornPos.position, transform.position);
        }
        if (m_MonsterBornPos != null && m_MonsterBornPos.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < m_MonsterBornPos.Length; i++)
            {
                Gizmos.DrawSphere(m_MonsterBornPos[i].position, 1f);
                Gizmos.DrawLine(m_MonsterBornPos[i].position, transform.position);
                Gizmos.color = Color.gray;
                Gizmos.DrawWireCube(m_MonsterBornPos[i].position, new Vector3(m_MonsterBornPos[i].localScale.x, m_MonsterBornPos[i].localScale.y, m_MonsterBornPos[i].localScale.z));
            }
        }

    }
#endif
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡场景门控制器
/// </summary>
public class GameLevelDoorCtrl : MonoBehaviour
{


    public GameLevelDoorCtrl m_ConenctDoor;
    [HideInInspector]
    public int OwnerRegionId;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (m_ConenctDoor != null)
        {
            Gizmos.DrawLine(transform.position, m_ConenctDoor.transform.position);
        }

    }
#endif
}

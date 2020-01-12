using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WroldMapTransCtrl : MonoBehaviour
{

    /// <summary>
    /// ����ID
    /// </summary>
    private int m_TransId;

    /// <summary>
    /// ����Ŀ�곡��
    /// </summary>
    private int m_TargetTransSceneId;

    /// <summary>
    /// ����Ŀ��Id
    /// </summary>
    private int m_TargetTransId;


    /// <summary>
    /// ���ò���
    /// </summary>
    /// <param name="transId"></param>
    /// <param name="targetTransSceneId"></param>
    /// <param name="targetTransId"></param>
    public void SetParameter(int transId, int targetTransSceneId, int targetTransId)
    {
        m_TransId = transId;
        m_TargetTransSceneId = targetTransSceneId;
        m_TargetTransId = targetTransId;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            SceneMgr.Instance.LoadWorldMap(m_TargetTransSceneId);
            SceneMgr.Instance.TargetTransId = m_TargetTransId;
        }
    }
}

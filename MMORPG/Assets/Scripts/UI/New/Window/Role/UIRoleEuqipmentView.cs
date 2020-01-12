using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleEuqipmentView : UISubViewBase
{
    [SerializeField]
    private Transform m_RoleModelContainer;

    [SerializeField]
    private Text m_NickNameText;
    [SerializeField]
    private Text m_LevelText;
    [SerializeField]
    private Text m_Fighting;

    private int m_Job;

    protected override void OnStart()
    {
        base.OnStart();
        CloneRoleModel();
    }
    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        m_RoleModelContainer = null;
        m_NickNameText = null;
        m_LevelText = null;
        m_Fighting = null;
    }
    public void SetUI(DataTransfer data)
    {
        m_Job = data.GetData<int>(ConstDefine.JobId);
        string nickName = data.GetData<string>(ConstDefine.NickName);
        int level = data.GetData<int>(ConstDefine.Level);
        int fighting = data.GetData<int>(ConstDefine.Fighting);
        m_NickNameText.text = nickName;
        m_LevelText.text = string.Format("Lv:{0}", level.ToString());
        m_Fighting.text = fighting.ToString();
    }

    public void CloneRoleModel()
    {
        GameObject go = RoleMgr.Instance.LoadPlayer(m_Job);
        go = GameObject.Instantiate(go);
        if (go != null)
        {
            go.transform.SetParent(m_RoleModelContainer);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.eulerAngles = Vector3.zero;
            Transform[] transArr = go.GetComponentsInChildren<Transform>();
            for (int i = 0; i < transArr.Length; i++)
            {
                transArr[i].gameObject.layer = LayerMask.NameToLayer("UI");
            }
            go.GetComponent<RoleCtrl>().enabled = false;
        }
    }
}

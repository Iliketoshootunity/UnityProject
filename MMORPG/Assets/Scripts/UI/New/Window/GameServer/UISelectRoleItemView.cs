using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectRoleItemView : MonoBehaviour
{


    [SerializeField]
    private Text m_NameText;
    [SerializeField]
    private Text m_LevelText;
    [SerializeField]
    private Text m_JobText;
    [SerializeField]
    private Button m_Button;

    private RoleOperation_LogOnGameServerReturnProto.RoleItem data;


    public Action<RoleOperation_LogOnGameServerReturnProto.RoleItem> OnClickItem;

    // Use this for initialization
    void Start()
    {
        EventTriggerListener.Get(m_Button.gameObject).onClick += OnClick;
    }

    private void OnClick(GameObject go)
    {
        if (OnClickItem != null)
        {
            OnClickItem(data);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        m_NameText = null;
        m_LevelText = null;
        m_JobText = null;
        m_Button = null;
    }

    public void SetUI(RoleOperation_LogOnGameServerReturnProto.RoleItem item)
    {
        m_NameText.text = item.NickName;
        m_LevelText.text = string.Format("Lv:{0}", item.Level);
        m_JobText.text = JobDBModel.Instance.Get((byte)item.RoleJob).Name;
        data = item;
    }
}

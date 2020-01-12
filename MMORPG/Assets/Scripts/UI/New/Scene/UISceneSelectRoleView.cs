using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneSelectRoleView : UISceneViewBase
{

    public Text JobNameText;

    public Text JobDesText;

    public InputField NickNameInput;

    public Action<bool> NextJobButton;

    public Action EnterGameButton;

    public Action DeleteRoleButton;

    public Action CreateRoleButton;

    public Action ReturnButton;

    public Transform[] CreateRoleUI;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnLeft":
                if (NextJobButton != null)
                {
                    NextJobButton(true);
                }

                break;
            case "btnRight":
                if (NextJobButton != null)
                {
                    NextJobButton(false);
                }
                break;
            case "btnEnterGame":
                if (EnterGameButton != null)
                {
                    EnterGameButton();
                }
                break;
            case "btnDeleteRole":
                if (DeleteRoleButton != null)
                {
                    DeleteRoleButton();
                    Debug.Log("btnDeleteRole");
                }
                break;
            case "btnCreateRole":
                if (CreateRoleButton != null)
                {
                    CreateRoleButton();
                }
                break;
            case "BtnReturn":
                if (ReturnButton != null)
                {
                    ReturnButton();
                }
                break;

        }

    }

    public void SetDes(string jobName, string jobDes)
    {
        JobNameText.text = jobName;
        JobDesText.text = jobDes;
    }

    public void ShowCreateRoleUI(bool isShow)
    {
        for (int i = 0; i < CreateRoleUI.Length; i++)
        {
            CreateRoleUI[i].gameObject.SetActive(isShow);
        }
    }

    public Transform[] SelectRoleUI;

    [SerializeField]
    private GameObject m_SelectRoleItemPrefab;

    [SerializeField]
    private Transform m_SelectRoleItemGrid;

    public Action<RoleOperation_LogOnGameServerReturnProto.RoleItem> OnClickSelectRoleItem;

    private List<GameObject> m_SelectRoleItemlist = new List<GameObject>();

    protected override void OnStart()
    {
        base.OnStart();
        for (int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(m_SelectRoleItemPrefab);
            m_SelectRoleItemlist.Add(go);
            go.transform.SetParent(m_SelectRoleItemGrid.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            UISelectRoleItemView itemView = m_SelectRoleItemlist[i].GetComponent<UISelectRoleItemView>();
            if (itemView != null)
            {
                itemView.OnClickItem = OnClickItem;
            }
            go.SetActive(false);
        }
    }
    public void ShowSelectRoleUI(bool isShow)
    {
        for (int i = 0; i < SelectRoleUI.Length; i++)
        {
            SelectRoleUI[i].gameObject.SetActive(isShow);
        }

    }

    public void SetRoleList(List<RoleOperation_LogOnGameServerReturnProto.RoleItem> roleList)
    {
        for (int i = 0; i < 10; i++)
        {
            m_SelectRoleItemlist[i].SetActive(false);
        }
        if (roleList != null && roleList.Count != 0)
        {
            for (int i = 0; i < roleList.Count; i++)
            {
                m_SelectRoleItemlist[i].SetActive(true);
                UISelectRoleItemView itemView = m_SelectRoleItemlist[i].GetComponent<UISelectRoleItemView>();
                if (itemView != null)
                {
                    itemView.SetUI(roleList[i]);
                }
            }
        }
    }

    private void OnClickItem(RoleOperation_LogOnGameServerReturnProto.RoleItem obj)
    {
        if (OnClickSelectRoleItem != null)
        {
            OnClickSelectRoleItem(obj);
        }
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();

        JobNameText = null;
        JobDesText = null;
        NickNameInput = null;
        NextJobButton = null;
        EnterGameButton = null;
        DeleteRoleButton = null;
        CreateRoleButton = null;
        ReturnButton = null;
        CreateRoleUI.SetArrNull();

        SelectRoleUI.SetArrNull();
        m_SelectRoleItemPrefab = null;
        OnClickSelectRoleItem = null;
        m_SelectRoleItemlist = null;
}
}

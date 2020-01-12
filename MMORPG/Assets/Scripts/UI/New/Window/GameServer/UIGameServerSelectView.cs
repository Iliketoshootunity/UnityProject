using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameServerSelectView : UIWindowViewBase
{
    #region 页签网格
    [SerializeField]
    private GameObject m_GameServerPagePrefab;

    [SerializeField]
    private GameObject m_GameServerPageGrid;

    public Action<int> OnClickPage;
    /// <summary>
    /// 设置页签网格
    /// </summary>
    /// <param name="pageList"></param>
    public void SetGameServerPageGrid(List<RetGameServerPageEntity> pageList)
    {
        if (pageList != null)
        {
            for (int i = 0; i < pageList.Count + 1; i++)
            {
                GameObject go = Instantiate(m_GameServerPagePrefab);
                go.transform.SetParent(m_GameServerPageGrid.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                UIGameServerPageView pageView = go.GetComponent<UIGameServerPageView>();
                if (pageView != null)
                {
                    if (i == 0)
                    {
                        RetGameServerPageEntity entity = new RetGameServerPageEntity();
                        entity.Name = "推荐服务器";
                        pageView.SetUI(entity);
                    }
                    else
                    {
                        pageView.OnClickGameServerPage = OnClickGameServerPage;
                        pageView.SetUI(pageList[i - 1]);
                    }
                }
            }
        }
    }


    private void OnClickGameServerPage(int obj)
    {
        if (OnClickPage != null)
        {
            OnClickPage(obj);
        }
    }

    #endregion

    [SerializeField]
    private GameObject m_GameServerItemPrefab;

    [SerializeField]
    private GameObject m_GameServerGrid;

    private List<GameObject> m_CacheGameServerItemList = new List<GameObject>();
    public Action<RetGameserverEntity> OnClickItem;

    [SerializeField]
    private UIGameServerItemView m_CurrentGameServer;

    protected override void OnStart()
    {
        base.OnStart();
        for (int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(m_GameServerItemPrefab);
            m_CacheGameServerItemList.Add(go);
            go.transform.SetParent(m_GameServerGrid.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActive(false);
        }
    }
    /// <summary>
    /// 设置服务器网格
    /// </summary>
    /// <param name="obj"></param>
    public void SetGameServerGrid(List<RetGameserverEntity> gameServerlist)
    {
        if (gameServerlist != null)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i > gameServerlist.Count - 1)
                {
                    m_CacheGameServerItemList[i].SetActive(false);
                }
            }
            for (int i = 0; i < gameServerlist.Count; i++)
            {
                GameObject go = m_CacheGameServerItemList[i];
                go.SetActive(true);
                UIGameServerItemView itemView = go.GetComponent<UIGameServerItemView>();
                if (itemView != null)
                {
                    itemView.SetUI(gameServerlist[i]);
                    itemView.OnClickGameServerItem = OnClickGameServerItem;
                }


            }
        }
    }

    public void SetCurrentGameServe(RetGameserverEntity entity)
    {
        m_CurrentGameServer.SetUI(entity);
    }

    private void OnClickGameServerItem(RetGameserverEntity obj)
    {
        if (OnClickItem != null)
        {
            OnClickItem(obj);
        }
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();


        m_GameServerPagePrefab = null;
        m_GameServerPageGrid = null;
        OnClickPage = null;

        m_GameServerItemPrefab = null;
        m_GameServerGrid = null;
        m_CacheGameServerItemList = null;
        OnClickItem = null;
        m_CurrentGameServer = null;


    }
}


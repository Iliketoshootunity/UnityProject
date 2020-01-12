using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏服务器控制器
/// </summary>
public class GameServerCtrl : SystemCtrlBase<GameServerCtrl>, ISystemCtrl
{

    private UIGameServerEnterView m_GameServerEnterView;
    private UIGameServerSelectView m_GameServerSelectView;

    private Dictionary<int, List<RetGameserverEntity>> pageTpGameServerList = new Dictionary<int, List<RetGameserverEntity>>();
    private int currentIndex = 0;
    public GameServerCtrl()
    {
        AddEventListen(ConstDefine.UIGameServerEnterView_btnGameServerEnter, GameServerEnterViewGameServerEnterBtnClick);
        AddEventListen(ConstDefine.UIGameServerEnterView_btnGameServerSelect, GameServerEnterViewGameServerSelectBtnClick);
    }



    public void OpenView(UIViewType type)
    {
        switch (type)
        {
            case UIViewType.GameServerEnter:
                OpenGameServerEnterView();
                break;
            case UIViewType.GameServerSelect:
                OpenGameServerSelectView();
                break;
        }

    }

    #region 服务器进入视图
    /// <summary>
    /// 打开服务器进入视图
    /// </summary>
    private void OpenGameServerEnterView()
    {
        m_GameServerEnterView = UIViewUtil.Instance.OpenWindow(UIViewType.GameServerEnter).GetComponent<UIGameServerEnterView>();
        m_GameServerEnterView.SetUI(Global.Instance.GameServerEntiry.Name);
    }

    /// <summary>
    /// 点击游戏服务器选择按钮
    /// </summary>
    /// <param name="p"></param>
    private void GameServerEnterViewGameServerSelectBtnClick(object[] p)
    {
        OpenGameServerSelectView();
        m_GameServerSelectView.OnLoadComplete = () => { GetGameServerPageList(); };
    }

    /// <summary>
    /// 点击进入游戏按钮
    /// </summary>
    /// <param name="p"></param>
    private void GameServerEnterViewGameServerEnterBtnClick(object[] p)
    {
        NetWorkSocket.Instance.OnConnectOk = OnConnectOkCallBack;
        NetWorkSocket.Instance.Connect(Global.Instance.GameServerEntiry.Ip, 1038);
    }
    private void OnConnectOkCallBack()
    {
        UpdatelastServer(Global.Instance.AccountEntity, Global.Instance.GameServerEntiry);
        SceneMgr.Instance.LoadSelectRole();
    }

    /// <summary>
    /// 获取页签
    /// </summary>
    private void GetGameServerPageList()
    {
        Dictionary<string, object> msg = new Dictionary<string, object>();
        msg.Add("Type", "1");
        NetWorkHttp.Instance.SendData(Global.Instance.WeAccountUrl + "api/gameserver", GetGanmeServerPageListCallBack, isPost: true, dic: msg);
    }
    /// <summary>
    /// 更新进入的最后的服务器
    /// </summary>
    /// <param name="accountEntity"></param>
    /// <param name="serverEntity"></param>
    private void UpdatelastServer(RetAccountEntity accountEntity, RetGameserverEntity serverEntity)
    {
        Dictionary<string, object> msg = new Dictionary<string, object>();
        msg.Add("Type", "2");
        msg.Add("UserID", accountEntity.Id);
        msg.Add("LastServerId", serverEntity.Id);
        msg.Add("LastServerName", serverEntity.Name);
        NetWorkHttp.Instance.SendData(Global.Instance.WeAccountUrl + "api/gameserver", UpdateLastServerCallBack, isPost: true, dic: msg);
    }
    /// <summary>
    ///  获取服务器页签列表回调
    /// </summary>
    /// <param name="obj"></param>
    private void GetGanmeServerPageListCallBack(CallBackArgs obj)
    {
        if (obj.IsError)
        {
            ShowMsg("Error", "Error");
        }
        else
        {
            List<RetGameServerPageEntity> list = LitJson.JsonMapper.ToObject<List<RetGameServerPageEntity>>(obj.Json);
            if (list == null)
            {
                ShowMsg("Error", "Error");
            }
            else
            {
                m_GameServerSelectView.SetGameServerPageGrid(list);
                GetGameServerList(0);
            }
        }
    }
    /// <summary>
    /// 更新最后登录的服务器回调
    /// </summary>
    /// <param name="obj"></param>
    private void UpdateLastServerCallBack(CallBackArgs obj)
    {
        if (!obj.IsError)
        {

        }
        else
        {
            Debug.Log("Update Over");
        }
    }
    #endregion

    #region 服务器选择视图
    /// <summary>
    /// 打开服务器选择视图
    /// </summary>
    private void OpenGameServerSelectView()
    {
        m_GameServerSelectView = UIViewUtil.Instance.OpenWindow(UIViewType.GameServerSelect).GetComponent<UIGameServerSelectView>();
        m_GameServerSelectView.OnClickPage = OnClickPage;
        m_GameServerSelectView.OnClickItem = OnClickItem;

    }

    /// <summary>
    /// 点击了页签
    /// </summary>
    private void OnClickPage(int pageIndex)
    {
        currentIndex = pageIndex;
        if (pageTpGameServerList.ContainsKey(currentIndex))
        {
            m_GameServerSelectView.SetGameServerGrid(pageTpGameServerList[currentIndex]);
        }
        else
        {
            GetGameServerList(currentIndex);
        }

    }

    /// <summary>
    /// 点击服务器物体
    /// </summary>
    /// <param name="retData"></param>
    private void OnClickItem(RetGameserverEntity retData)
    {
        m_GameServerSelectView.Close(); Global.Instance.GameServerEntiry = retData;
        m_GameServerEnterView.SetUI(retData.Name);
    }


    /// <summary>
    /// 获取服务器列表
    /// </summary>
    /// <param name="pageIndex"></param>
    private void GetGameServerList(int pageIndex)
    {
        Dictionary<string, object> msg = new Dictionary<string, object>();
        msg.Add("Type", "0");
        msg.Add("PageIndex", pageIndex);
        NetWorkHttp.Instance.SendData(Global.Instance.WeAccountUrl + "api/gameserver", GetGameServerListCallBack, isPost: true, dic: msg);
    }


    /// <summary>
    /// 获取服务器列表回调
    /// </summary>
    /// <param name="obj"></param>
    private void GetGameServerListCallBack(CallBackArgs obj)
    {
        if (obj.IsError)
        {
            ShowMsg("Error", "Error");
        }
        else
        {
            List<RetGameserverEntity> list = LitJson.JsonMapper.ToObject<List<RetGameserverEntity>>(obj.Json);
            pageTpGameServerList[currentIndex] = list;
            if (list == null)
            {
                ShowMsg("Error", "Error");
            }
            else
            {
                m_GameServerSelectView.SetGameServerGrid(list);
            }
        }
    }

    public override void Dispose()
    {
        RemoveEventListen(ConstDefine.UIGameServerEnterView_btnGameServerEnter, GameServerEnterViewGameServerEnterBtnClick);
        RemoveEventListen(ConstDefine.UIGameServerEnterView_btnGameServerSelect, GameServerEnterViewGameServerSelectBtnClick);
    }
    #endregion
}

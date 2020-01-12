using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ������������
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

    #region ������������ͼ
    /// <summary>
    /// �򿪷�����������ͼ
    /// </summary>
    private void OpenGameServerEnterView()
    {
        m_GameServerEnterView = UIViewUtil.Instance.OpenWindow(UIViewType.GameServerEnter).GetComponent<UIGameServerEnterView>();
        m_GameServerEnterView.SetUI(Global.Instance.GameServerEntiry.Name);
    }

    /// <summary>
    /// �����Ϸ������ѡ��ť
    /// </summary>
    /// <param name="p"></param>
    private void GameServerEnterViewGameServerSelectBtnClick(object[] p)
    {
        OpenGameServerSelectView();
        m_GameServerSelectView.OnLoadComplete = () => { GetGameServerPageList(); };
    }

    /// <summary>
    /// ���������Ϸ��ť
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
    /// ��ȡҳǩ
    /// </summary>
    private void GetGameServerPageList()
    {
        Dictionary<string, object> msg = new Dictionary<string, object>();
        msg.Add("Type", "1");
        NetWorkHttp.Instance.SendData(Global.Instance.WeAccountUrl + "api/gameserver", GetGanmeServerPageListCallBack, isPost: true, dic: msg);
    }
    /// <summary>
    /// ���½�������ķ�����
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
    ///  ��ȡ������ҳǩ�б�ص�
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
    /// ��������¼�ķ������ص�
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

    #region ������ѡ����ͼ
    /// <summary>
    /// �򿪷�����ѡ����ͼ
    /// </summary>
    private void OpenGameServerSelectView()
    {
        m_GameServerSelectView = UIViewUtil.Instance.OpenWindow(UIViewType.GameServerSelect).GetComponent<UIGameServerSelectView>();
        m_GameServerSelectView.OnClickPage = OnClickPage;
        m_GameServerSelectView.OnClickItem = OnClickItem;

    }

    /// <summary>
    /// �����ҳǩ
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
    /// �������������
    /// </summary>
    /// <param name="retData"></param>
    private void OnClickItem(RetGameserverEntity retData)
    {
        m_GameServerSelectView.Close(); Global.Instance.GameServerEntiry = retData;
        m_GameServerEnterView.SetUI(retData.Name);
    }


    /// <summary>
    /// ��ȡ�������б�
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
    /// ��ȡ�������б�ص�
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

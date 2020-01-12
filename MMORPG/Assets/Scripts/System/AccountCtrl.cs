using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountCtrl : SystemCtrlBase<AccountCtrl>, ISystemCtrl
{

    private UILogOnView logOnView;
    private UIRegView regView;

    private bool isAutoLogOn;
    public AccountCtrl()
    {
        AddEventListen(ConstDefine.UILogOnView_btnLogon, LogOnViewLogOnBtnClick);
        AddEventListen(ConstDefine.UILogOnView_btnReg, LogOnViewRegBtnClick);

        AddEventListen(ConstDefine.UIRegView_btnReg, RegViewRegBtnClick);
        AddEventListen(ConstDefine.UIRegView_btnToLogon, RegViewToLogOnBtnClick);
    }


    /// <summary>
    /// 快速登录
    /// </summary>
    public void QuickLogOn()
    {
        if (!PlayerPrefs.HasKey(ConstDefine.PlayerPrefs_AccountID_Key))
        {
            OpenView(UIViewType.Reg);
            return;
        }
        else
        {
            isAutoLogOn = true;
            //登录请求
            string userName = PlayerPrefs.GetString(ConstDefine.PlayerPrefs_UserName_Key);
            string pwd = PlayerPrefs.GetString(ConstDefine.PlayerPrefs_Pwd_Key);
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add("Type", "1");
            msg.Add("UserName", userName);
            msg.Add("Pwd", pwd);
            NetWorkHttp.Instance.SendData(Global.Instance.WeAccountUrl + "api/account", OnLogOnCallBack, isPost: true, dic: msg);
        }

    }

    public void OpenView(UIViewType type)
    {
        switch (type)
        {
            case UIViewType.LogOn:
                OpenLogOnView();
                break;
            case UIViewType.Reg:
                OpenRegView();
                break;
        }

    }



    /// <summary>
    /// 打开登录视图
    /// </summary>
    private void OpenLogOnView()
    {
        logOnView = UIViewUtil.Instance.OpenWindow(UIViewType.LogOn).GetComponent<UILogOnView>();
        isAutoLogOn = false;
    }

    /// <summary>
    /// 打开注册视图
    /// </summary>
    private void OpenRegView()
    {
        regView = UIViewUtil.Instance.OpenWindow(UIViewType.Reg).GetComponent<UIRegView>();
        isAutoLogOn = false;
    }
    #region 按钮事件


    /********************************************
     * 登录视图
     * *****************************************/

    /// <summary>
    /// 登录视图登录按钮
    /// </summary>
    /// <param name="p"></param>
    private void LogOnViewLogOnBtnClick(object[] p)
    {
        if (string.IsNullOrEmpty(logOnView.UserNameInput.text))
        {
            MessageCtrl.Instance.Show("登录提示", "用户名不能为空");
            return;
        }
        if (string.IsNullOrEmpty(logOnView.PwdInput.text))
        {
            MessageCtrl.Instance.Show("登录提示", "密码不能为空");
            return;
        }

        string userName = logOnView.UserNameInput.text;
        string pwd = logOnView.PwdInput.text;
        Dictionary<string, object> msg = new Dictionary<string, object>();
        msg.Add("Type", "1");
        msg.Add("UserName", userName);
        msg.Add("Pwd", pwd);
        NetWorkHttp.Instance.SendData(Global.Instance.WeAccountUrl + "api/account", OnLogOnCallBack, isPost: true, dic: msg);
    }

    private void OnLogOnCallBack(CallBackArgs obj)
    {
        if (obj.IsError)
        {
            Debug.Log("发送消息错误");
        }
        else
        {
            RetValue ret = LitJson.JsonMapper.ToObject<RetValue>(obj.Json);
            if (ret.IsError)
            {
                Debug.Log(ret.ErrorMsg);
            }
            else
            {
                RetAccountEntity accountEntity = LitJson.JsonMapper.ToObject<RetAccountEntity>(ret.RetData.ToString());
                Global.Instance.AccountEntity = accountEntity;
                SetLastGameServer(accountEntity);
                if (isAutoLogOn)
                {
                    UIViewMgr.Instance.OpenView(UIViewType.GameServerEnter);
                }
                else
                {
                    string userName = logOnView.UserNameInput.text;
                    string pwd = logOnView.PwdInput.text;
                    PlayerPrefs.SetInt(ConstDefine.PlayerPrefs_AccountID_Key, accountEntity.Id);
                    PlayerPrefs.SetString(ConstDefine.PlayerPrefs_UserName_Key, userName);
                    PlayerPrefs.SetString(ConstDefine.PlayerPrefs_Pwd_Key, pwd);
                    logOnView.CloseAndOpenNextView(UIViewType.GameServerEnter);
                }

                Debug.Log("登录成功" + ret.RetData);
            }
        }
    }

    /// <summary>
    /// 登录视图 注册按钮
    /// </summary>
    /// <param name="p"></param>
    private void LogOnViewRegBtnClick(object[] p)
    {
        logOnView.CloseAndOpenNextView(UIViewType.Reg);
    }


    /********************************************
     * 注册视图
     * *****************************************/
    /// <summary>
    /// 注册视图返回登录按钮
    /// </summary>
    /// <param name="p"></param>
    private void RegViewToLogOnBtnClick(object[] p)
    {
        regView.CloseAndOpenNextView(UIViewType.LogOn);

    }
    /// <summary>
    /// 注册视图 注册按钮
    /// </summary>
    /// <param name="p"></param>
    private void RegViewRegBtnClick(object[] p)
    {
        if (string.IsNullOrEmpty(regView.UserNameInput.text))
        {
            MessageCtrl.Instance.Show("注册提示", "用户名不能为空");
            return;
        }
        if (string.IsNullOrEmpty(regView.PwdInput.text))
        {
            MessageCtrl.Instance.Show("注册提示", "密码不能为空");
            return;
        }
        if (string.IsNullOrEmpty(regView.YanZhenInput.text))
        {
            MessageCtrl.Instance.Show("注册提示", "密码不能为空");
            return;
        }
        string userName = regView.UserNameInput.text;
        string pwd = regView.PwdInput.text;
        Dictionary<string, object> msg = new Dictionary<string, object>();
        msg.Add("Type", "0");
        msg.Add("UserName", userName);
        msg.Add("Pwd", pwd);
        msg.Add("Channelld", "123");
        NetWorkHttp.Instance.SendData(Global.Instance.WeAccountUrl + "api/account", OnRegCallBack, isPost: true, dic: msg);
    }

    private void OnRegCallBack(CallBackArgs obj)
    {
        if (obj.IsError)
        {
            Debug.Log("发送消息失败");
        }
        else
        {
            RetValue ret = LitJson.JsonMapper.ToObject<RetValue>(obj.Json);
            if (ret.IsError)
            {
                Debug.Log(ret.ErrorMsg);
            }
            else
            {
                RetAccountEntity accountEntity = LitJson.JsonMapper.ToObject<RetAccountEntity>(ret.RetData.ToString());
                Global.Instance.AccountEntity = accountEntity;
                SetLastGameServer(accountEntity);
                if (isAutoLogOn)
                {

                }
                else
                {
                    string userName = regView.UserNameInput.text;
                    string pwd = regView.PwdInput.text;
                    PlayerPrefs.SetInt(ConstDefine.PlayerPrefs_AccountID_Key, accountEntity.Id);
                    PlayerPrefs.SetString(ConstDefine.PlayerPrefs_UserName_Key, userName);
                    PlayerPrefs.SetString(ConstDefine.PlayerPrefs_Pwd_Key, pwd);
                }
                regView.CloseAndOpenNextView(UIViewType.GameServerEnter);
                Debug.Log("注册成功" + ret.RetData);
            }
        }
    }
    #endregion 按钮事件

    private void SetLastGameServer(RetAccountEntity accountEntity)
    {
        RetGameserverEntity serverEntity = new RetGameserverEntity();
        serverEntity.Id = accountEntity.LastServerId;
        serverEntity.Name = accountEntity.LastServerName;
        serverEntity.Ip = accountEntity.LastServerIP;
        serverEntity.Port = accountEntity.LastServerPort;
        Global.Instance.GameServerEntiry = serverEntity;
    }
    public override void Dispose()
    {
        RemoveEventListen(ConstDefine.UILogOnView_btnLogon, RegViewToLogOnBtnClick);
        RemoveEventListen(ConstDefine.UILogOnView_btnReg, LogOnViewRegBtnClick);
        Instance.RemoveEventListen(ConstDefine.UIRegView_btnReg, RegViewRegBtnClick);
        Instance.RemoveEventListen(ConstDefine.UIRegView_btnToLogon, RegViewToLogOnBtnClick);
    }
}

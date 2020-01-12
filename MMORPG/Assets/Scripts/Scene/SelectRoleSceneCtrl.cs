using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择场景控制器
/// </summary>
public class SelectRoleSceneCtrl : MonoBehaviour
{
    public Transform RolePoint;
    private List<JobEntity> m_JobList = null;
    private List<RoleOperation_LogOnGameServerReturnProto.RoleItem> m_RoleItemList = new List<RoleOperation_LogOnGameServerReturnProto.RoleItem>();
    private UISceneSelectRoleView selectRoleView;
    private int selectIndex;
    private GameObject oldObj;
    private bool isCreateRole;
    private int CurrentRoleID;
    private int LastWorldMapId;

    private bool m_EnterGameOK;
    private bool m_GetSelectRoleInfoOK;
    private bool m_GetSkillInfoOK;
    private bool m_CreateRoleOK;
    void Start()
    {
        selectRoleView = UISceneCtrl.Instance.Load(UISceneType.SelectRole).GetComponent<UISceneSelectRoleView>();
        selectRoleView.OnClickSelectRoleItem = OnClickSelectRoleItem;
        selectRoleView.NextJobButton = OnClickNextJobButton;
        selectRoleView.EnterGameButton = OnClickEnterGameButton;
        selectRoleView.DeleteRoleButton = OnClickDeleteRoleButton;
        selectRoleView.CreateRoleButton = OnClickCreateRoleButton;
        selectRoleView.ReturnButton = OnClickReturnButton;

        LoadRole();

        RoleOpration_LogOnGameServerProto proto = new RoleOpration_LogOnGameServerProto();
        proto.AccoutID = Global.Instance.AccountEntity.Id;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.LogOnGameServerReturnProto, OnLogOnGameServerReturnProto);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.DeleteRoleReturnProto, OnDeleteRoleReturnProto);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.EnterGameReturnProto, OnEnterGameReturnProto);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.SelectRoleInfoReturnProto, OnSelectRoleInfoReturnProto);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.CreateRoleReturnProto, OnCreateRoleReturnProto);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.SkillReturnReturnProto, OnSkillReturnReturnProto);
        //加载完成
        if (DelegateDefine.Instance.OnLoadSceneOK != null)
        {
            DelegateDefine.Instance.OnLoadSceneOK();
        }

    }

    private void OnSkillReturnReturnProto(byte[] p)
    {
        RoleData_SkillReturnProto proto = RoleData_SkillReturnProto.ToPoto(p);
        if (proto != null)
        {
            m_GetSkillInfoOK = true;
            Global.Instance.CurRoleInfo.LoadSkill(proto);
        }

    }

    private void Update()
    {
        if ((m_EnterGameOK && m_GetSelectRoleInfoOK && m_GetSkillInfoOK) || (m_CreateRoleOK && m_GetSelectRoleInfoOK && m_GetSkillInfoOK))
        {

            SceneMgr.Instance.LoadWorldMap(PlayerCtrl.Instance.LastWorldMapId);
        }
    }


    #region 按钮点击
    /// <summary>
    /// 点击进入游戏按钮
    /// </summary>
    private void OnClickEnterGameButton()
    {
        //创建角色界面点击进入游戏
        if (isCreateRole)
        {
            if (string.IsNullOrEmpty(selectRoleView.NickNameInput.text))
            {
                MessageCtrl.Instance.Show("创建角色提示", "昵称不能为空");
                return;
            }

            RoleOperation_CreateRoleProto proto = new RoleOperation_CreateRoleProto();
            proto.JobID = m_JobList[selectIndex].Id;
            proto.NickName = selectRoleView.NickNameInput.text;
            NetWorkSocket.Instance.SendMsg(proto.ToArray());

        }
        //选择界面点击进入游戏
        else
        {
            RoleOperation_EnterGameProto proto = new RoleOperation_EnterGameProto();
            proto.RoleID = CurrentRoleID;
            NetWorkSocket.Instance.SendMsg(proto.ToArray());

        }

    }
    /// <summary>
    /// 点击返回按钮
    /// </summary>
    private void OnClickReturnButton()
    {
        if (isCreateRole)
        {
            //如果角色数量大于0 则返回选择角色界面 否则返回登录界面
            if (m_RoleItemList.Count > 0)
            {
                ShowSelectRoleView();
            }
            else
            {
                SceneMgr.Instance.LoadLogin();
                NetWorkSocket.Instance.DisConnect();
            }
        }
        else
        {
            //否则返回登录界面
            SceneMgr.Instance.LoadLogin();
            NetWorkSocket.Instance.DisConnect();
        }
    }

    /// <summary>
    /// 点击创建角色按钮
    /// </summary>
    private void OnClickCreateRoleButton()
    {
        ShowCreateRoleView();
    }

    /// <summary>
    /// 点击角色列表中的一项
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickSelectRoleItem(RoleOperation_LogOnGameServerReturnProto.RoleItem obj)
    {
        CurrentRoleID = obj.RoleId;
        CloneRole(obj.RoleJob);
    }


    /// <summary>
    /// 点击下一个职业按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickNextJobButton(bool obj)
    {
        if (obj)
        {
            selectIndex--;
            if (selectIndex < 0)
            {
                selectIndex = m_JobList.Count - 1;
            }
        }
        else
        {
            selectIndex++;
            if (selectIndex > m_JobList.Count - 1)
            {
                selectIndex = 0;
            }
        }
        CloneRole(m_JobList[selectIndex].Id);
        string jobName = m_JobList[selectIndex].Name;
        string jobDes = m_JobList[selectIndex].Desc;
        selectRoleView.SetDes(jobName, jobDes);
    }

    /// <summary>
    /// 点击删除回调
    /// </summary>
    private void OnClickDeleteRoleButton()
    {
        MessageCtrl.Instance.Show("删除角色提示", "是否要删除这个角色", MsgButtonType.OkAndCancel, OnClickOK: OnClickDeleteOK);
    }

    /// <summary>
    /// 确定伤处角色
    /// </summary>
    private void OnClickDeleteOK()
    {
        RoleOperation_DeleteRoleProto proto = new RoleOperation_DeleteRoleProto();
        proto.RoleID = CurrentRoleID;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }
    #endregion
    #region 协议回调

    /// <summary>
    /// 选择的角色的信息 协议回调
    /// </summary>
    /// <param name="p"></param>
    private void OnSelectRoleInfoReturnProto(byte[] p)
    {
        RoleOperation_SelectRoleInfoReturnProto proto = RoleOperation_SelectRoleInfoReturnProto.ToProto(p);
        m_GetSelectRoleInfoOK = proto.IsSucess;
        if (proto.IsSucess)
        {
            PlayerCtrl.Instance.LastWorldMapId = proto.LastSceneId;
            Global.Instance.CurRoleInfo.LoadRoleInfo(proto);
        }
        else
        {

        }
    }
    /// <summary>
    /// 登录服务器协议回调
    /// </summary>
    /// <param name="p"></param>
    private void OnLogOnGameServerReturnProto(byte[] p)
    {
        RoleOperation_LogOnGameServerReturnProto proto = RoleOperation_LogOnGameServerReturnProto.ToPoto(p);
        m_RoleItemList = proto.Roles;
        if (proto.RoleCount > 0)
        {
            ShowSelectRoleView();
        }
        else
        {
            ShowCreateRoleView();
        }
    }


    /// <summary>
    /// 删除角色协议回调
    /// </summary>
    /// <param name="p"></param>
    private void OnDeleteRoleReturnProto(byte[] p)
    {
        RoleOperation_DeleteRoleReturnProto proto = RoleOperation_DeleteRoleReturnProto.ToProto(p);
        if (proto.IsSucess)
        {
            RoleOperation_LogOnGameServerReturnProto.RoleItem roleItem = m_RoleItemList.Find((item) => item.RoleId == CurrentRoleID);
            if (roleItem != null)
            {
                m_RoleItemList.Remove(roleItem);
                if (m_RoleItemList.Count > 0)
                {
                    CurrentRoleID = m_RoleItemList[0].RoleId;
                }
                else
                {
                    ShowCreateRoleView();
                    return;
                }
            }
            else
            {
                Debug.Log("Error");
            }

            ShowSelectRoleView();
        }
        else
        {
            Debug.Log("Error:0000");
        }

    }
    /// <summary>
    /// 进入游戏协议回调
    /// </summary>
    /// <param name="p"></param>
    private void OnEnterGameReturnProto(byte[] p)
    {
        RoleOperation_EnterGameReturnProto proto = RoleOperation_EnterGameReturnProto.ToProto(p);
        m_EnterGameOK = proto.IsSucess;
        if (proto.IsSucess)
        {
            Debug.Log("进入游戏成功");
        }
    }

    /// <summary>
    /// 创建角色 结果 协议回调
    /// </summary>
    /// <param name="p"></param>
    private void OnCreateRoleReturnProto(byte[] p)
    {
        RoleOperation_CreateRoleReturnProto proto = RoleOperation_CreateRoleReturnProto.ToProto(p);
        m_CreateRoleOK = proto.IsSucess;
    }
    #endregion
    private void LoadRole()
    {
        m_JobList = JobDBModel.Instance.GetList();
        for (int i = 0; i < m_JobList.Count; i++)
        {
            GameObject go = AssetBundleMgr.Instance.Load(string.Format(@"download\prefab\roleprefab\player\{0}.assetbundle", m_JobList[i].PrefabName), m_JobList[i].PrefabName);
            Global.Instance.RolePrefab.Add(m_JobList[i].Id, go);
        }
    }



    private void CloneRole(int jobId)
    {
        if (oldObj != null)
        {
            Destroy(oldObj);
        }
        GameObject go = Instantiate(Global.Instance.RolePrefab[jobId]);
        go.GetComponent<RoleCtrl>().enabled = false;
        go.transform.SetParent(RolePoint);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        oldObj = go;
    }


    /// <summary>
    /// 显示选择角色界面
    /// </summary>
    private void ShowSelectRoleView()
    {
        //选择角色
        isCreateRole = false;
        selectRoleView.ShowCreateRoleUI(false);
        selectRoleView.ShowSelectRoleUI(true);
        selectRoleView.SetRoleList(m_RoleItemList);
        //默认显示第一个拥有的职业
        CloneRole(m_RoleItemList[0].RoleJob);
    }

    /// <summary>
    /// 显示创建角色界面
    /// </summary>
    private void ShowCreateRoleView()
    {
        //创建角色 
        isCreateRole = true;
        selectRoleView.ShowCreateRoleUI(true);
        selectRoleView.ShowSelectRoleUI(false);
        //默认显示第一个职业
        CloneRole(m_JobList[0].Id);
        selectIndex = 0;
        string jobName = m_JobList[0].Name;
        string jobDes = m_JobList[0].Desc;
        selectRoleView.SetDes(jobName, jobDes);

    }






    private void OnDestroy()
    {
        RolePoint = null;
        m_JobList = null;
        m_RoleItemList = null;
        selectRoleView = null;
        oldObj = null;
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.LogOnGameServerReturnProto, OnLogOnGameServerReturnProto);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.CreateRoleReturnProto, OnCreateRoleReturnProto);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.EnterGameReturnProto, OnEnterGameReturnProto);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.SelectRoleInfoReturnProto, OnSelectRoleInfoReturnProto);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.CreateRoleReturnProto, OnCreateRoleReturnProto);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.SkillReturnReturnProto, OnSkillReturnReturnProto);
    }
}

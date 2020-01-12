using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public bool IsDeug;

    #region 常量
    public static readonly string MMO_NICKNAME;
    public static readonly string MMO_PASSWORD;
    #endregion

    #region 玩家信息

    public string CurNickName;
    public RoleCtrl CurPlayer;
    /// <summary>
    /// 角色镜像
    /// </summary>
    public Dictionary<int, GameObject> RolePrefab = new Dictionary<int, GameObject>();

    /// <summary>
    /// 主角信息
    /// </summary>
    public RoleInfoMainPlayer CurRoleInfo = new RoleInfoMainPlayer();

    /// <summary>
    /// 服务器时间
    /// </summary>
    private long m_ServerTime;

    /// <summary>
    /// 当前服务器时间
    /// </summary>
    public long CurrentServerTime
    {
        get
        {
            return m_ServerTime + (long)Time.time;
        }
    }


    #endregion



    public static Global Instance;
    public AnimationCurve UIAnimationCurve;

    [HideInInspector]
    public string WeAccountUrl = "http://192.168.1.4:8080/";
    private void Awake()
    {
        Instance = this;
        WeAccountUrl = "http://192.168.1.4:8080/";
        DontDestroyOnLoad(gameObject);
    }

    [HideInInspector]
    public RetAccountEntity AccountEntity;
    [HideInInspector]
    public RetGameserverEntity GameServerEntiry;
    private void Start()
    {
        NetWorkHttp.Instance.SendData(WeAccountUrl + "api/time", GetServerTimeCallBack);
        Debug.Log("我的名字");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.DeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            CurPlayer.CurRoleInfo.CurrentHP = 10;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CurPlayer.Attack.IsAutoFight = !CurPlayer.Attack.IsAutoFight;
        }
    }
    private void GetServerTimeCallBack(CallBackArgs obj)
    {
        if (obj.IsError)
        {
            Debug.Log(obj.Error);
        }
        else
        {
            m_ServerTime = obj.Json.ToLong();
            Debug.Log(m_ServerTime);
        }
    }
}

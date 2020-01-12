using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkHttp : MonoBehaviour
{

    private static NetWorkHttp _instance;

    public static NetWorkHttp Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("NetWorkHttp");
                if (go == null)
                {
                    go = new GameObject("NetWorkHttp");
                }
                _instance = go.GetOrCreateComponen<NetWorkHttp>();
            }
            return _instance;
        }
    }


    private Action<CallBackArgs> callBackAction;

    private CallBackArgs callBackArgs;

    private bool m_IsBuy = false;
    private void Start()
    {
        callBackArgs = new CallBackArgs();
    }


    public void SendData(string url, Action<CallBackArgs> callBack, bool isPost = false, Dictionary<string, object> dic = null)
    {
        if (m_IsBuy) return;
        m_IsBuy = true;
        this.callBackAction = callBack;
        if (isPost)
        {
            if (dic == null)
            {
                dic = new Dictionary<string, object>();
            }
            //设备标识符
            dic["deviceIdentifier"] = DeviceUtil.DeviceIdentifier;
            //设备型号
            dic["DeviceModel"] = DeviceUtil.DeviceModel;
            //签名
            dic["sign"] = string.Format("{0},{1}", DeviceUtil.DeviceIdentifier, Global.Instance.CurrentServerTime);
            //时间戳
            dic["t"] = Global.Instance.CurrentServerTime.ToString();
            PostUrl(url, JsonMapper.ToJson(dic));
        }
        else
        {
            GetUrl(url);
        }
    }
    #region Get
    private void GetUrl(string url)
    {
        WWW date = new WWW(url);
        StartCoroutine(GetUrlIE(date));
    }

    private IEnumerator GetUrlIE(WWW date)
    {
        yield return date;
        m_IsBuy = false;
        if (!string.IsNullOrEmpty(date.error))
        {
            callBackArgs.IsError = true;
            callBackArgs.Error = date.error;
        }
        else
        {
            if (date.text == "null")
            {
                callBackArgs.IsError = true;
                callBackArgs.Error = "未找到数据";
            }
            else
            {
                callBackArgs.IsError = false;
                callBackArgs.Json = date.text;
            }
        }

        if (callBackAction != null)
        {
            callBackAction(callBackArgs);
        }
    }

    #endregion
    private void PostUrl(string url, string json)
    {
        //表单
        WWWForm form = new WWWForm();
        //添加字段
        form.AddField("", json);

        WWW date = new WWW(url, form);
        StartCoroutine(PostUrlIE(date));
    }

    private IEnumerator PostUrlIE(WWW date)
    {
        yield return date;
        m_IsBuy = false;
        if (!string.IsNullOrEmpty(date.error))
        {
            callBackArgs.IsError = true;
            callBackArgs.Error = date.error;
        }
        else
        {
            if (date.text == "null")
            {
                callBackArgs.IsError = true;
                callBackArgs.Error = "传送的数据为空";
            }
            else
            {
                callBackArgs.IsError = false;
                callBackArgs.Json = date.text;
            }
        }

        if (callBackAction != null)
        {
            callBackAction(callBackArgs);
        }
    }


}

public class CallBackArgs
{
    public bool IsError;

    public string Error;

    public string Json;
}


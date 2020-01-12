using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 进入游戏视图
/// </summary>
public class UIGameServerEnterView : UIWindowViewBase
{
    public Text DefaultGameServerText;

    public void SetUI(string defaultGameServer)
    {
        if (DefaultGameServerText != null)
        {
            DefaultGameServerText.text = defaultGameServer;
        }
    }
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnGameServerSelect":
                UIDispatcher.Instance.Dispatc(ConstDefine.UIGameServerEnterView_btnGameServerSelect, null);
                break;
            case "btnGameServerEnter":
                UIDispatcher.Instance.Dispatc(ConstDefine.UIGameServerEnterView_btnGameServerEnter, null);
                break;
        }

    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        DefaultGameServerText = null;
    }

}

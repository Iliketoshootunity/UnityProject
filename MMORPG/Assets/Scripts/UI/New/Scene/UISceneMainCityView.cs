using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneMainCityView : UISceneViewBase
{
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnMenuShow":
                ChangeMenuStatus(go);
                break;
            case "btnRole":
                UIViewMgr.Instance.OpenView(UIViewType.RoleInfo);
                break;
            case "btnGameLevelMap":
                UIViewMgr.Instance.OpenView(UIViewType.GameLevelMap);
                break;
        }
    }


    public void ChangeMenuStatus(GameObject go)
    {
        UIMenuView.Instance.ChangeStatus(() => { go.transform.localScale = new Vector3(go.transform.localScale.x * -1, go.transform.localScale.y, go.transform.localScale.z); });
    }

}

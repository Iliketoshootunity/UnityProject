using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIRegView : UIWindowViewBase
{
    public InputField UserNameInput;
    public InputField PwdInput;
    public InputField YanZhenInput;
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnReg":
                UIDispatcher.Instance.Dispatc(ConstDefine.UIRegView_btnReg, null);
                break;
            case "btnToLogon":
                UIDispatcher.Instance.Dispatc(ConstDefine.UIRegView_btnToLogon, null);
                break;
        }

    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        UserNameInput = null;
        PwdInput = null;
        YanZhenInput = null;
    }
}

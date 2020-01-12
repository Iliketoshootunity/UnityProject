using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILogOnView : UIWindowViewBase
{
    public InputField UserNameInput;
    public InputField PwdInput;
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnLogon":
                UIDispatcher.Instance.Dispatc(ConstDefine.UILogOnView_btnLogon, null);
                break;
            case "btnReg":
                UIDispatcher.Instance.Dispatc(ConstDefine.UILogOnView_btnReg, null);
                break;
        }

    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        UserNameInput = null;
        PwdInput = null;
    }
}

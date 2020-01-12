using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameLevelFailView : UIWindowViewBase
{

    public Action OnbtnReturnWroldScene;
    public Action OnbtnResurrection;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnReturnWroldScene":
                if (OnbtnReturnWroldScene != null)
                {
                    OnbtnReturnWroldScene();
                }
                break;
            case "btnResurrection":
                if (OnbtnResurrection != null)
                {
                    OnbtnResurrection();
                }
                break;
        }
    }

}

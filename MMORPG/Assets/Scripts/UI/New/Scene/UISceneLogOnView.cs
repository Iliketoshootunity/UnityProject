using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLogOnView : UISceneViewBase
{

    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine("OnStartDely");
    }


    private IEnumerator OnStartDely()
    {
        yield return new WaitForSeconds(1f);
        AccountCtrl.Instance.QuickLogOn();

    }
}

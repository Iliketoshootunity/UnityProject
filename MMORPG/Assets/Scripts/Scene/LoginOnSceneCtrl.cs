using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������
/// </summary>
public class LoginOnSceneCtrl : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GameObject obj = UISceneCtrl.Instance.Load(UISceneType.LogOn);
        //�������
        if (DelegateDefine.Instance.OnLoadSceneOK != null)
        {
            DelegateDefine.Instance.OnLoadSceneOK();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

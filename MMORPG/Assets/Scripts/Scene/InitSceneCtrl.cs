using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneCtrl : MonoBehaviour
{

    private void Start()
    {
        UISceneCtrl.Instance.Load(UISceneType.Init);
        StartCoroutine(LoadNextScene());

    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3);
        SceneMgr.Instance.LoadLogin();
    }

}

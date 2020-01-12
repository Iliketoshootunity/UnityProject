using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingSceneCtrl : MonoBehaviour
{
    private int progress;
    private AsyncOperation operation;
    private UILoadingSceneCtrl uiLoading;
    // Use this for initialization
    void Start()
    {
        //if (SceneMgr.Instance.CurrentType != SceneType.LogOn)
        //{
        //    AssetBundleMgr.Instance.LoadScene(string.Format("download/scene/{0}.scene", SceneMgr.Instance.CurrentType.ToString()), SceneMgr.Instance.CurrentType.ToString(), OnLoadComplete);
        //}
        //else
        //{
        //    operation = SceneManager.LoadSceneAsync(SceneMgr.Instance.CurrentType.ToString(),LoadSceneMode.Additive);
        //    operation.allowSceneActivation = false;
        //}
        UIViewUtil.Instance.CloseAll();
        LoadNextScene();
        uiLoading = UISceneCtrl.Instance.Load(UISceneType.Loading).GetComponent<UILoadingSceneCtrl>();
        DelegateDefine.Instance.OnLoadSceneOK += OnLoadSceneOK;
    }

    private void LoadNextScene()
    {
        if (SceneMgr.Instance.CurrentType == SceneType.WorldMap)
        {
            string strSceneName = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurWorldMapSceneId).SceneName;
            operation = SceneManager.LoadSceneAsync(strSceneName);
        }
        else if (SceneMgr.Instance.CurrentType == SceneType.Gamelevel)
        {
            string strSceneName = GameLevelDBModel.Instance.Get(SceneMgr.Instance.CurGameLevelSceneId).SceneName;
            operation = SceneManager.LoadSceneAsync(strSceneName);
        }
        else
        {
            operation = SceneManager.LoadSceneAsync(SceneMgr.Instance.CurrentType.ToString());
        }

        operation.allowSceneActivation = false;
    }

    /// <summary>
    /// 场景加载完成
    /// </summary>
    private void OnLoadSceneOK()
    {
        Destroy(uiLoading.gameObject);
    }


    private void OnLoadComplete()
    {
        operation = SceneManager.LoadSceneAsync(SceneMgr.Instance.CurrentType.ToString());
        operation.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (operation == null || uiLoading == null) return;
        int toProgress = 0;
        if (operation.progress < 0.9f)
        {
            toProgress = Mathf.Clamp(toProgress * 100, 1, 101);
        }
        else
        {
            toProgress = 100;
        }
        if (progress < toProgress)
        {
            progress++;
        }
        else
        {
            operation.allowSceneActivation = true;
        }
        uiLoading.SetProgressValue(progress * 0.01f);
    }

    private void OnDestroy()
    {
        DelegateDefine.Instance.OnLoadSceneOK -= OnLoadSceneOK;
        operation = null;
        uiLoading = null;
    }
}

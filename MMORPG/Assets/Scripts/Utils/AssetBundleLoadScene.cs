using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleLoadScene : MonoBehaviour
{


    private string m_fullPath;
    private string m_name;
    private string m_AssetPath;

    public Action OnLoadComplete;
    public void Init(string assetPath, string sourceName)
    {
        m_AssetPath = assetPath;
        m_fullPath = LocalFileMgr.localFilePath + assetPath; ;
        m_name = sourceName;
    }
    // Use this for initialization
    void Start()
    {
        StartCoroutine(load());
    }

    private void OnDestroy()
    {

    }
    private IEnumerator load()
    {
        //加载manifast文件
        AssetBundle manifestAssetBundle = null;
        AssetBundleManifest manifest = load<AssetBundleManifest>(LocalFileMgr.Platform, "AssetBundleManifest", ref manifestAssetBundle);
        //加载依赖项
        string[] depends = manifest.GetAllDependencies(m_AssetPath);
        AssetBundle[] dependAssetBundles = new AssetBundle[depends.Length];
        for (int i = 0; i < depends.Length; i++)
        {
            AssetBundle dependAssetBundle = null;
            load<UnityEngine.Object>(depends[i], depends[i], ref dependAssetBundle);
            dependAssetBundles[i] = dependAssetBundle;
        }

        WWW www = WWW.LoadFromCacheOrDownload(m_fullPath, 0);
        yield return www;
        AssetBundle bundle = null;
        if (www.error == null)
        {
            bundle = www.assetBundle;

            if (OnLoadComplete != null)
            {
                OnLoadComplete();
            }
        }
        else
        {
            Debug.LogError(www.error);
        }
        if (manifestAssetBundle != null)
        {
            manifestAssetBundle.Unload(false);
        }
        www = null;
        for (int i = 0; i < dependAssetBundles.Length; i++)
        {
            if (dependAssetBundles != null)
            {
                dependAssetBundles[i].Unload(false);
            }

        }
        if (bundle != null)
        {
            bundle.Unload(false);
        }

    }

    private T load<T>(string assetName, string sourName, ref AssetBundle bundle) where T : UnityEngine.Object
    {

        bundle = AssetBundle.LoadFromFile(string.Format("{0}{1}", LocalFileMgr.localFilePath, assetName));
        return bundle.LoadAsset(sourName) as T;
    }
}

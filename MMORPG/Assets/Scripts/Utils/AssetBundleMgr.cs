using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class AssetBundleMgr : Singleton<AssetBundleMgr>
{

    public GameObject Load(string path, string name)
    {
        using (AssetBundleLoder loader = new AssetBundleLoder(path, name))
        {
            return loader.Load<GameObject>();
        }
    }

    public GameObject LoadClone(string path, string name)
    {
        using (AssetBundleLoder loader = new AssetBundleLoder(path, name))
        {
            GameObject prefab = loader.Load<GameObject>();
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab);
            }
            else
            {
                return null;
            }

        }
    }

    public void LoadScene(string path, string name, Action loadComplete)
    {
        GameObject go = new GameObject();
        AssetBundleLoadScene scnen = go.GetOrCreateComponen<AssetBundleLoadScene>();
        scnen.Init(path, name);
        scnen.OnLoadComplete = loadComplete;
    }


}

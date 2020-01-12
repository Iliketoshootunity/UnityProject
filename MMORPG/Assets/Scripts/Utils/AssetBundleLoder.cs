using UnityEngine;
using System.Collections;
using System;

public class AssetBundleLoder : System.IDisposable
{

    private AssetBundle bundle;
    private string m_AssetPath;
    private string m_sourceName;
    public AssetBundleLoder(string assetPath, string sourceNameh)
    {
        this.m_AssetPath = assetPath;
        this.m_sourceName = sourceNameh;
    }

    public T Load<T>() where T : UnityEngine.Object
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
        //
        T go = load<T>(m_AssetPath, m_sourceName, ref bundle);
        manifestAssetBundle.Unload(false);
        for (int i = 0; i < dependAssetBundles.Length; i++)
        {
            dependAssetBundles[i].Unload(false);
        }
        return go;
    }
    public void Dispose()
    {
        if (bundle == null) return;
        bundle.Unload(false);
    }
    private T load<T>(string assetName, string sourName, ref AssetBundle bundle) where T : UnityEngine.Object
    {

        bundle = AssetBundle.LoadFromFile(string.Format("{0}{1}", LocalFileMgr.localFilePath, assetName));
        return bundle.LoadAsset(sourName) as T;
    }




}

using UnityEngine;
using System.Collections;
using System;

public class AssetBundleLoaderAsync : MonoBehaviour
{

    private string m_fullPath;
    private string m_name;

    private AssetBundleCreateRequest request;
    private AssetBundle assetBounle;

    public Action<UnityEngine.Object> OnLoadComplete;
    public void Init(string fullPath, string name)
    {
        m_fullPath = LocalFileMgr.localFilePath + fullPath; ;
        m_name = name;
    }
    // Use this for initialization
    void Start()
    {
        StartCoroutine("load");
    }

    private void OnDestroy()
    {
        if (assetBounle != null)
        {
            assetBounle.Unload(false);
        }
    }
    private IEnumerator load()
    {
        request = AssetBundle.LoadFromMemoryAsync(LocalFileMgr.Instance.GetBuffer(m_fullPath));
        yield return request;
        assetBounle = request.assetBundle;
        if (assetBounle != null)
        {
            if (OnLoadComplete != null)
            {
                OnLoadComplete(assetBounle.LoadAsset(m_name));
            }
        }
        Destroy(this.gameObject);
    }

}

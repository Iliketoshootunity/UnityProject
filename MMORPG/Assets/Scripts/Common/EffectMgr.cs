using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EffectMgr : Singleton<EffectMgr>
{
    private MonoBehaviour m_Mono;
    private SpawnPool m_EffectPool;
    private Dictionary<string, Transform> m_Dic;
    public void Init(MonoBehaviour mono)
    {
        this.m_Mono = mono;
        m_Dic = new Dictionary<string, Transform>();
        m_EffectPool = PoolManager.Pools.Create("Effect");
        Debug.Log("Thread id ：" + Thread.CurrentThread.ManagedThreadId);
    }
    private GameObject LoadEffect(string effectName)
    {
        GameObject go = AssetBundleMgr.Instance.Load(string.Format(@"download\prefab\effect\role\{0}.assetbundle", effectName), effectName);
        return go;
    }

    public Transform PlayEffect(string effectName)
    {

        if (!m_Dic.ContainsKey(effectName))
        {
            Transform t = LoadEffect(effectName).transform;
            if (t == null) return null;
            m_Dic[effectName] = t;
            PrefabPool prefabPool = new PrefabPool(t);
            prefabPool.preloadAmount = 0;
            prefabPool.cullDespawned = true;
            prefabPool.cullAbove = 5;
            prefabPool.cullDelay = 2;
            prefabPool.cullMaxPerPass = 2;
            m_EffectPool.CreatePrefabPool(prefabPool);
        }
        return m_EffectPool.Spawn(m_Dic[effectName]);
    }

    public void DestoryEffect(Transform effect, float delay)
    {
        m_Mono.StartCoroutine(DestoryEffectIE(effect, delay));
    }

    private IEnumerator DestoryEffectIE(Transform effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        m_EffectPool.Despawn(effect);
    }
    public void Clear()
    {
        m_EffectPool = null;
        m_Mono = null;
        m_Dic = null;
    }

}

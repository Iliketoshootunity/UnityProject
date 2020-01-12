using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UITipView : UISubViewBase
{
    public static UITipView Instance;

    private Queue<TipEntity> m_TipQueue = new Queue<TipEntity>();

    private float m_PreTipTime;

    private SpawnPool m_TipPool;

    private GameObject prefab;
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
    }

    protected override void OnStart()
    {
        base.OnStart();
        prefab = ResourcesMrg.Instance.Load(ResourcesMrg.ResourceType.UIOther, "UITipViewItem", isCache: true, isClone: false);
        if (prefab != null)
        {
            m_TipPool = PoolManager.Pools.Create("Tip");
            m_TipPool.group.parent = null;
            m_TipPool.group.localPosition = Vector3.zero;
            Transform t = prefab.transform;
            PrefabPool prefabPool = new PrefabPool(t);
            prefabPool.preloadAmount = 5;
            prefabPool.cullDespawned = true;
            prefabPool.cullAbove = 5;
            prefabPool.cullDelay = 2;
            prefabPool.cullMaxPerPass = 2;
            m_TipPool.CreatePrefabPool(prefabPool);
        }
    }
    private void Update()
    {
        if (Time.time > m_PreTipTime + 0.5f)
        {
            if (m_TipQueue.Count > 0)
            {
                TipEntity entity = m_TipQueue.Dequeue();
                Transform t = m_TipPool.Spawn(prefab.transform);
                UITipViewItem item = t.GetComponent<UITipViewItem>();
                if (item != null)
                {
                    item.SetUI(entity);
                }
                t.SetParent(transform);
                t.localPosition = Vector3.zero;
                t.localScale = Vector3.one;
                t.localRotation = Quaternion.identity;
                t.DOLocalMoveY(150, 0.5f).SetEase(Ease.Linear).OnComplete(() => { m_TipPool.Despawn(t); });
            }
            m_PreTipTime = Time.time;
        }

    }
    public void SetUI(int type, string value)
    {
        TipEntity entity = new TipEntity();
        entity.Type = type;
        entity.Value = value;
        m_TipQueue.Enqueue(entity);
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        m_TipPool = null;
    }
}


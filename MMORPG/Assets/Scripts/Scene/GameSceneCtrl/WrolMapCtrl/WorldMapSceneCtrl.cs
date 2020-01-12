using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 世界地图场景控制器 
/// </summary>
public class WorldMapSceneCtrl : GameSceneCtrlBase
{

    /// <summary>
    /// 玩家角色位置
    /// </summary>
    public Transform PlayerRolePos;

    private Dictionary<int, Transform> m_TransPosDic;
    protected override void OnAwake()
    {
        base.OnAwake();
    }
    protected override void OnMainUIComplete()
    {
        RoleMgr.Instance.InitRoleMain();
        PlayerCtrl.Instance.SetMainCityData();
        Vector3 point = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurWorldMapSceneId).GetBornPoint;
        float y = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurWorldMapSceneId).GetBornRotateY;
        InitTrans();
        if (SceneMgr.Instance.TargetTransId == 0)
        {
            if (point == Vector3.zero)
            {
                if (Global.Instance.CurPlayer != null)
                {
                    Global.Instance.CurPlayer.Born(PlayerRolePos.position);
                }
            }
            else
            {
                Global.Instance.CurPlayer.Born(point);
                Global.Instance.CurPlayer.transform.eulerAngles = new Vector3(0, y, 0);
            }
        }
        else
        {
            if (m_TransPosDic.ContainsKey(SceneMgr.Instance.TargetTransId))
            {
                Vector3 bornPos = m_TransPosDic[SceneMgr.Instance.TargetTransId].forward.normalized * 3 + m_TransPosDic[SceneMgr.Instance.TargetTransId].position;
                Vector3 lookPos = m_TransPosDic[SceneMgr.Instance.TargetTransId].forward.normalized * 3.5f + m_TransPosDic[SceneMgr.Instance.TargetTransId].position;
                Global.Instance.CurPlayer.Born(bornPos);
                Global.Instance.CurPlayer.transform.LookAt(lookPos);
                SceneMgr.Instance.TargetTransId = 0;
            }

        }

        StartCoroutine(InitNPC());
        //加载完成
        if (DelegateDefine.Instance.OnLoadSceneOK != null)
        {
            DelegateDefine.Instance.OnLoadSceneOK();
        }
    }

    /// <summary>
    /// 初始化NPC
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitNPC()
    {
        yield return null;
        List<WorldMapNpcData> npcList = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurWorldMapSceneId).NPCDataList;
        if (npcList != null && npcList.Count > 0)
        {
            for (int i = 0; i < npcList.Count; i++)
            {
                NPCEntity entity = NPCDBModel.Instance.Get(npcList[i].NPCId);
                GameObject go = RoleMgr.Instance.LoadNpc(entity.PrefabName);
                go.transform.position = npcList[i].NpcBornPoint;
                go.transform.eulerAngles = new Vector3(0, npcList[i].NpCBornRotateY, 0);

                NPCCtrl npc = go.GetComponent<NPCCtrl>();
                npc.Init(npcList[i]);
            }
        }
    }

    /// <summary>
    /// 初始化传送点
    /// </summary>
    private void InitTrans()
    {
        m_TransPosDic = new Dictionary<int, Transform>();
        //传送点（坐标_y轴旋转_传送点编号_要传送的场景Id_目标场景出生传送点id）
        WorldMapEntity entity = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurWorldMapSceneId);
        if (string.IsNullOrEmpty(entity.TransPos)) return;
        string[] transArr = entity.TransPos.Split('|');
        for (int i = 0; i < transArr.Length; i++)
        {
            string[] transEntity = transArr[i].Split('_');
            if (transEntity.Length != 7) continue;
            Vector3 pos = new Vector3();
            float f = 0;
            float.TryParse(transArr[0], out f);
            pos.x = f;
            float.TryParse(transArr[1], out f);
            pos.y = f;
            float.TryParse(transArr[2], out f);
            pos.z = f;
            float rotateY = 0;
            float.TryParse(transArr[3], out rotateY);
            int transId = 0;
            int.TryParse(transArr[4], out transId);
            int targetSceneId = 0;
            int.TryParse(transArr[5], out targetSceneId);
            int targetTransId = 0;
            int.TryParse(transArr[6], out targetTransId);
            GameObject go = ResourcesMrg.Instance.Load(ResourcesMrg.ResourceType.Effect, "EffectTrans", isCache: true, isClone: true);
            if (go != null)
            {
                go.transform.position = pos;
                go.transform.eulerAngles = new Vector3(0, rotateY, 0);
                WroldMapTransCtrl transCtrl = go.GetComponent<WroldMapTransCtrl>();
                if (transCtrl != null)
                {
                    transCtrl.SetParameter(transId, targetSceneId, targetTransId);
                }
                if (!m_TransPosDic.ContainsKey(transId))
                {
                    m_TransPosDic[transId] = go.transform;
                }
            }
        }
    }
    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
    }

}

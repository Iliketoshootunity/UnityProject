using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleMgr : Singleton<RoleMgr>
{

    private bool m_InitMainPlayer;

    public void InitRoleMain()
    {
        if (m_InitMainPlayer) return;
        m_InitMainPlayer = true;
        if (Global.Instance.CurRoleInfo != null)
        {
            GameObject go = GameObject.Instantiate(Global.Instance.RolePrefab[Global.Instance.CurRoleInfo.JobId]);
            if (go != null)
            {
                Global.Instance.CurPlayer = go.GetComponent<RoleCtrl>();
                GameObject.DontDestroyOnLoad(go);
                if (Global.Instance.CurPlayer != null)
                {
                    Global.Instance.CurPlayer.Init(RoleType.MainPlayer, Global.Instance.CurRoleInfo, new RoleMainPlayerCityAI(Global.Instance.CurPlayer));
                    Global.Instance.CurRoleInfo.MaxMP = (Global.Instance.CurRoleInfo.CurrentMP *= 10);
                    Global.Instance.CurRoleInfo.LoadPhyAttack(Global.Instance.CurRoleInfo.JobId);
                }

            }
        }

    }

    /// <summary>
    /// 加载角色
    /// </summary>
    /// <returns></returns>
    public GameObject LoadRole(RoleType roleType, string name)
    {
        GameObject go = null;
        string path = "";
        switch (roleType)
        {
            case RoleType.MainPlayer:
                path = "Player";
                break;
            case RoleType.Monster:
                path = "Monster";
                break;
        }
        return go = ResourcesMrg.Instance.Load(ResourcesMrg.ResourceType.Role, string.Format("{0}/{1}", path, name)); ;

    }

    /// <summary>
    /// 加载玩家
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public GameObject LoadPlayer(int jobId)
    {
        GameObject prefab = Global.Instance.RolePrefab[jobId];
        return prefab;
    }

    /// <summary>
    /// 加载技能图片
    /// </summary>
    /// <param name="picName"></param>
    /// <returns></returns>
    public Sprite LoadSkillPic(string picName)
    {
        return Resources.Load<Sprite>(string.Format("UI/SkillPic/{0}", picName));
    }
    public GameObject LoadNpc(string prefrabName)
    {
        GameObject go = AssetBundleMgr.Instance.Load(string.Format(@"download\prefab\roleprefab\npc\{0}.assetbundle", prefrabName), prefrabName);
        return GameObject.Instantiate(go);
    }

}

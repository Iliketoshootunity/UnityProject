using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUtil
{

    public static Texture LoadGameLevelMapBG(string name)
    {
        return Resources.Load<Texture>(string.Format("UI/GameLevel/GameLevelMap/{0}", name));
    }

    public static Sprite LoadGameLevelItemBG(string name)
    {
        return Resources.Load<Sprite>(string.Format("UI/GameLevel/GameLevelIcon/{0}", name));
    }
    public static Sprite LoadGameLevelDlgBG(string name)
    {
        return Resources.Load<Sprite>(string.Format("UI/GameLevel/GameLevelDlg/{0}", name));
    }
    public static Sprite LoadGameIcon(string name, GameLevelRewardType rewardType)
    {
        string path = string.Empty;
        switch (rewardType)
        {
            case GameLevelRewardType.Equip:
                path = "EquipIcon";
                break;
            case GameLevelRewardType.Item:
                path = "ItemIcon";
                break;
            case GameLevelRewardType.Material:
                path = "MaterialIcon";
                break;
        }

        return Resources.Load<Sprite>(string.Format("UI/{0}/{1}", path, name));
    }

    public static GameObject LoadSprite(int spriteId)
    {
        string prefabName = SpriteDBModel.Instance.Get(spriteId).PrefabName;
        GameObject go = AssetBundleMgr.Instance.Load(string.Format(@"download\prefab\roleprefab\monster\{0}.assetbundle", prefabName), prefabName);
        return go;
    }
    /// <summary>
    /// 圆形范围内找点
    /// </summary>
    /// <param name="target"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static Vector3 GetTargetOuterRingPoint(Vector3 target, float range)
    {
        Vector3 v = new Vector3(0, 0, 1);
        Quaternion q = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        v = (q * v).normalized;
        v = v * range + target;
        return v;
    }
    /// <summary>
    /// 扇形范围内找点
    /// </summary>
    /// <param name="target"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static Vector3 GetTargetSectorPoint(Vector3 curPos, Vector3 target, float range)
    {
        Vector3 v = (curPos - target).normalized;
        Quaternion q = Quaternion.Euler(0, UnityEngine.Random.Range(-90, 90), 0);
        v = (q * v).normalized;
        v = v * range + target;
        return v;
    }
}

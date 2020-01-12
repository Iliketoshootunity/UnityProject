using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏关卡场景控制器
/// </summary>
public class GameLevelSceneCtrl : GameSceneCtrlBase
{
    /// <summary>
    /// 所有的区域
    /// </summary>
    [SerializeField]
    private GameLevelRegionCtrl[] m_AllRegion;
    private List<GameLevelRegionEntity> regionEntityList;
    private int m_GameLevelId;
    private GameLevelGrade m_Grade;
    private int m_CurSelectRegionIndex;
    private int m_CurSelectRegionId;
    /// <summary>
    /// 所有怪的ID
    /// </summary>
    private int[] m_AllMonsterID;

    /// <summary>
    /// 所有怪的数量
    /// </summary>
    private int m_AllMonsterCount;

    /// <summary>
    /// 当前区域怪的数量
    /// </summary>
    private int m_CurrentRegionMonsterCount;

    /// <summary>
    /// 杀死的区域怪的数量
    /// </summary>
    private int m_KillRegionMonsterCount;

    /// <summary>
    /// 当前区域怪的信息
    /// </summary>
    private List<GameLevelMonsterEntity> m_CurrentRegionMonsterList;
    /// <summary>
    /// 缓存池
    /// </summary>
    private SpawnPool m_Pool;

    private GameLevelRegionCtrl m_CurRegionCtrl;
    /// <summary>
    /// 创建
    /// </summary>
    private int m_CurrentCreateCount;

    /// <summary>
    /// 是否在战斗
    /// </summary>
    private bool m_IsFighting;
    /// <summary>
    /// 战斗时间
    /// </summary>
    private float m_FightingTime;


    /// <summary>
    /// 当前区域是否有怪
    /// </summary>
    public bool CurRegionHasMonster
    {
        get
        {
            return m_KillRegionMonsterCount < m_CurrentRegionMonsterCount;
        }
    }
    /// <summary>
    /// 是否是最后一个区域
    /// </summary>
    public bool LastRegion
    {
        get
        {
            return m_CurSelectRegionIndex > regionEntityList.Count - 1;
        }
    }

    public Vector3 NextRegionPlyaerPos
    {
        get
        {
            GameLevelRegionEntity entiy = GetRegionEntityByIndex(m_CurSelectRegionIndex);
            if (entiy == null) return Vector3.zero; ;
            int regionId = entiy.RegionId;
            m_CurRegionCtrl = GetRegionCtrlByRegionIndex(regionId);
            if (m_CurRegionCtrl == null) return Vector3.zero;
            return m_CurRegionCtrl.RoleBornPos.position;
        }
    }

    public static GameLevelSceneCtrl Instance;

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
    }
    protected override void OnMainUIComplete()
    {
        base.OnMainUIComplete();
        m_IsFighting = true;
        PlayerCtrl.Instance.SetMainCityData();
        m_GameLevelId = SceneMgr.Instance.CurGameLevelSceneId;
        m_Grade = SceneMgr.Instance.CurGameLevelGrade;
        regionEntityList = GameLevelRegionDBModel.Instance.GetRegionListByGameLevelId(m_GameLevelId);
        m_CurSelectRegionIndex = 0;
        EnterRegion(m_CurSelectRegionIndex);
        m_AllMonsterCount = GameLevelMonsterDBModel.Instance.GetLevelMonsterCount(m_GameLevelId, m_Grade);
        m_AllMonsterID = GameLevelMonsterDBModel.Instance.GetLevelAllMonsterId(m_GameLevelId, m_Grade);
        m_Pool = PoolManager.Pools.Create("Monster");
        m_Pool.group.parent = null;
        m_Pool.group.localPosition = Vector3.zero;
        for (int i = 0; i < m_AllMonsterID.Length; i++)
        {
            Transform t = GameUtil.LoadSprite(m_AllMonsterID[i]).transform;
            PrefabPool prefabPool = new PrefabPool(t);
            prefabPool.preloadAmount = 5;
            prefabPool.cullDespawned = true;
            prefabPool.cullAbove = 5;
            prefabPool.cullDelay = 2;
            prefabPool.cullMaxPerPass = 2;
            m_Pool.CreatePrefabPool(prefabPool);
        }
        Global.Instance.CurPlayer.ToIdle(IdleType.IdleFight);
        GameLevelCtrl.Instance.CurGameLevelTotalExp = 0;
        GameLevelCtrl.Instance.CurGameLevelTotalGold = 0;
        GameLevelCtrl.Instance.CurGameLevelKillMonsterDic.Clear();
        GameLevelCtrl.Instance.CurGameLevelGetGoodsList.Clear();
    }



    protected override void OnStart()
    {
        base.OnStart();

    }
    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        m_Pool = null;
        m_CurrentRegionMonsterList.Clear();
        regionEntityList.Clear();
        m_CurRegionCtrl = null;
        m_AllRegion.SetArrNull();
    }

    private float m_CreateMonsterTimer;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (m_IsFighting)
        {
            if (m_CurrentCreateCount < m_CurrentRegionMonsterCount)
            {
                if (Time.time > m_CreateMonsterTimer)
                {
                    CreateMonster();
                    m_CreateMonsterTimer = Time.time + 1;
                }

            }
            m_FightingTime += Time.deltaTime;
        }

    }
    /// <summary>
    /// 进入区域
    /// </summary>
    /// <param name="index"></param>
    private void EnterRegion(int index)
    {
        GameLevelRegionEntity entiy = GetRegionEntityByIndex(index);
        if (entiy == null) return;
        m_KillRegionMonsterCount = 0;
        int regionId = entiy.RegionId;
        m_CurRegionCtrl = GetRegionCtrlByRegionIndex(regionId);
        if (m_CurRegionCtrl == null) return;
        if (index == 0)
        {
            if (Global.Instance.CurPlayer != null)
            {
                Global.Instance.CurPlayer.Born(m_CurRegionCtrl.RoleBornPos.position);
                Global.Instance.CurPlayer.OnRoleDie = (ctrl) => { UIViewMgr.Instance.OpenView(UIViewType.GameLevelFail); };
            }
        }
        if (m_CurRegionCtrl.RegionMaskObj != null)
        {
            Destroy(m_CurRegionCtrl.RegionMaskObj);
        }
        //开启一下个区域的门
        if (index != 0)
        {
            GameLevelDoorCtrl door = m_CurRegionCtrl.GetNextRegionDoor(m_CurSelectRegionId);
            if (door != null)
            {
                if (door.m_ConenctDoor != null)
                {
                    door.m_ConenctDoor.gameObject.SetActive(false);
                }
                door.gameObject.SetActive(false);
            }
        }
        m_CurSelectRegionId = regionId;
        m_CurrentRegionMonsterCount = GameLevelMonsterDBModel.Instance.GetRegionMonsterCount(m_GameLevelId, m_Grade, regionId);
        m_CurrentRegionMonsterList = GameLevelMonsterDBModel.Instance.GetRegionAllMonsterInfo(m_GameLevelId, m_Grade, regionId);
        m_CurrentCreateCount = 0;

        if (DelegateDefine.Instance.OnLoadSceneOK != null)
        {
            DelegateDefine.Instance.OnLoadSceneOK();
        }

    }
    private int m_Index;
    private int m_TempIndex;
    private Dictionary<int, int> monsterCreateDic = new Dictionary<int, int>();
    private void CreateMonster()
    {
        //if (m_CurrentCreateCount > 0) return;
        m_Index = UnityEngine.Random.Range(0, m_CurrentRegionMonsterList.Count);
        GameLevelMonsterEntity levelMonsterEntity = m_CurrentRegionMonsterList[m_Index];
        SpriteEntity spriteEntity = SpriteDBModel.Instance.Get(levelMonsterEntity.SpriteId);
        if (spriteEntity == null || levelMonsterEntity == null) return;
        if (!monsterCreateDic.ContainsKey(m_Index))
        {
            monsterCreateDic[m_Index] = levelMonsterEntity.SpriteCount;
        }
        string prefabName = spriteEntity.PrefabName;
        monsterCreateDic[m_Index]--;
        if (monsterCreateDic[m_Index]-- < 0)
        {
            monsterCreateDic.Remove(m_Index);
        }
        Transform obj = m_Pool.Spawn(prefabName);
        obj.position = m_CurRegionCtrl.GetMonsterBornPos;
        obj.localEulerAngles = Vector3.zero;
        obj.localScale = Vector3.one;
        RoleCtrl ctrl = obj.GetComponent<RoleCtrl>();
        ctrl.OnRoleDie = OnRoleDieCallBack;
        ctrl.OnRoleDestory = OnRoleDestoryCallBack;
        RoleInfoMonster info = new RoleInfoMonster(spriteEntity);
        info.RoleId = ++m_TempIndex;
        info.RoleNickName = spriteEntity.Name;
        info.Level = spriteEntity.Level;
        //info.Exp = spriteEntity.RewardExp;
        info.MaxHP = spriteEntity.HP;
        info.CurrentHP = spriteEntity.HP;
        info.MaxMP = spriteEntity.MP;
        info.CurrentMP = spriteEntity.MP;
        info.Attack = spriteEntity.Attack;
        info.Defense = spriteEntity.Defense;
        info.Hit = spriteEntity.Hit;
        info.Dodge = spriteEntity.Dodge;
        info.Res = spriteEntity.Res;
        info.Cri = spriteEntity.Cri;
        info.Fighting = spriteEntity.Fighting * 100;
        ctrl.Init(RoleType.Monster, info, new GameLevel_RoleMonsterAI(ctrl, info));
        ctrl.Born(obj.position);
        ctrl.ViewRange = spriteEntity.Range_View;
        m_CurrentCreateCount++;
    }

    private void OnRoleDieCallBack(RoleCtrl ctrl)
    {
        //GameLevelCtrl.Instance.CurPassTime = (int)m_FightingTime;
        //UIViewMgr.Instance.OpenView(UIViewType.GameLevelVictory);
        m_KillRegionMonsterCount++;
        RoleInfoMonster info = (RoleInfoMonster)ctrl.CurRoleInfo;
        if (info != null)
        {
            GameLevelMonsterEntity monsterEntity = GameLevelMonsterDBModel.Instance.GetRegionMonsterInfo(m_GameLevelId, m_Grade, m_CurRegionCtrl.RegionId, info.SpriteEntity.Id);
            if (monsterEntity != null)
            {
                if (monsterEntity.Exp > 0)
                {
                    UITipView.Instance.SetUI(0, monsterEntity.Exp.ToString());
                    GameLevelCtrl.Instance.CurGameLevelTotalExp += monsterEntity.Exp;
                }
                if (monsterEntity.Gold > 0)
                {
                    UITipView.Instance.SetUI(1, monsterEntity.Gold.ToString());
                    GameLevelCtrl.Instance.CurGameLevelTotalGold += monsterEntity.Gold;
                }
                if (GameLevelCtrl.Instance.CurGameLevelKillMonsterDic.ContainsKey(monsterEntity.SpriteId))
                {
                    GameLevelCtrl.Instance.CurGameLevelKillMonsterDic[monsterEntity.SpriteId] += 1;
                }
                else
                {
                    GameLevelCtrl.Instance.CurGameLevelKillMonsterDic[monsterEntity.SpriteId] = 1;
                }
            }
        }
        if (m_KillRegionMonsterCount >= m_CurrentRegionMonsterCount)
        {

            m_CurSelectRegionIndex++;
            if (LastRegion)
            {
                //胜利
                m_IsFighting = false;
                GameLevelCtrl.Instance.CurPassTime = (int)m_FightingTime;
                TimeMgr.Instance.SetTimeScaleDuration(0.3f, 3);
                StartCoroutine("FightingWinIE");
                return;
            }
            EnterRegion(m_CurSelectRegionIndex);
        }
    }
    /// <summary>
    /// 战斗胜利
    /// </summary>
    /// <returns></returns>
    private IEnumerator FightingWinIE()
    {
        yield return new WaitForSeconds(3);
        UIViewMgr.Instance.OpenView(UIViewType.GameLevelVictory);
    }
    private void OnRoleDestoryCallBack(Transform obj)
    {
        m_Pool.Despawn(obj);
    }

    /// <summary>
    /// 根据索引号得到区域实体
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private GameLevelRegionEntity GetRegionEntityByIndex(int index)
    {
        for (int i = 0; i < regionEntityList.Count; i++)
        {
            if (i == index)
            {
                return regionEntityList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 根据区域ID 获得区域控制器
    /// </summary>
    /// <param name="regionIndex"></param>
    /// <returns></returns>
    private GameLevelRegionCtrl GetRegionCtrlByRegionIndex(int regionIndex)
    {
        for (int i = 0; i < m_AllRegion.Length; i++)
        {
            if (m_AllRegion[i].RegionId == regionIndex)
            {
                return m_AllRegion[i];
            }
        }
        return null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(TestPoint, 1);
        Gizmos.DrawSphere(transform.position, 1f);
        if (m_AllRegion != null && m_AllRegion.Length > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < m_AllRegion.Length; i++)
            {
                Gizmos.DrawLine(transform.position, m_AllRegion[i].transform.position);
            }

        }
    }
#endif
}

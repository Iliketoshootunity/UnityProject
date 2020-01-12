using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelCtrl : SystemCtrlBase<GameLevelCtrl>, ISystemCtrl
{
    private UIGameLevelMapView m_GameLevelMapView;
    private UIGameLevelDetailsView m_GameLevelDetailsView;
    private UIGameLevelVictoryView m_GameLevelVictoryView;
    private UIGameLevelFailView m_GameLevelFailView;
    /// <summary>
    /// 当前关卡ID
    /// </summary>
    private int m_GameLevelSceneId;
    /// <summary>
    /// 当前难度等级
    /// </summary>
    private GameLevelGrade m_Garade;
    /// <summary>
    /// 当前的通关时间
    /// </summary>
    public int CurPassTime;
    /// <summary>
    /// 当前关卡所有经验
    /// </summary>
    public int CurGameLevelTotalExp;
    /// <summary>
    /// 当前关卡所有金币
    /// </summary>
    public int CurGameLevelTotalGold;
    /// <summary>
    /// 当前关卡杀死的小怪
    /// </summary>
    public Dictionary<int, int> CurGameLevelKillMonsterDic;
    /// <summary>
    /// 当前关卡杀死的小怪
    /// </summary>
    public List<GetGoodsEntity> CurGameLevelGetGoodsList;

    public GameLevelCtrl()
    {
        CurGameLevelKillMonsterDic = new Dictionary<int, int>();
        CurGameLevelGetGoodsList = new List<GetGoodsEntity>();

        AddEventListen(ConstDefine.UIGameLevelDetailsView_Grade, OnClickGameLevelDetailsViewGradeButton);
        AddEventListen(ConstDefine.UIGameLevelDetailsView_EnterLevel, OnClickGameLevelDetailsViewEnterLevelButton);

        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.GameLevel_EnterReturnProto, GameLevel_EnterReturnProtoCallBack);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.GameLevel_VictroyReturnProto, GameLevel_VictroyReturnProtoCallBack);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.GameLevel_FailReturnProto, GameLevel_FailReturnProtoCallBack);
        SocketDispatcher.Instance.AddEventListen(ProtoCodeDef.GameLevel_ResurgenceReturnProto, GameLevel_ResurgenceReturnProtoCallBack);
    }


    public override void Dispose()
    {
        base.Dispose();
        RemoveEventListen(ConstDefine.UIGameLevelDetailsView_Grade, OnClickGameLevelDetailsViewGradeButton);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.GameLevel_EnterReturnProto, GameLevel_EnterReturnProtoCallBack);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.GameLevel_VictroyReturnProto, GameLevel_VictroyReturnProtoCallBack);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.GameLevel_FailReturnProto, GameLevel_FailReturnProtoCallBack);
        SocketDispatcher.Instance.RemoveEventListen(ProtoCodeDef.GameLevel_ResurgenceReturnProto, GameLevel_ResurgenceReturnProtoCallBack);
    }

    public void OpenView(UIViewType type)
    {
        switch (type)
        {
            case UIViewType.GameLevelMap:
                OpenGameLevelMapView();
                break;
            case UIViewType.GameLevelVictory:
                OpenGameLevelVictoryView();
                break;
            case UIViewType.GameLevelFail:
                OpenGameLevelFail();
                break;
            default:
                break;
        }
    }
    #region 打开页面
    /// <summary>
    /// 打开管卡地图
    /// </summary>
    private void OpenGameLevelMapView()
    {
        m_GameLevelMapView = UIViewUtil.Instance.OpenWindow(UIViewType.GameLevelMap).GetComponent<UIGameLevelMapView>();
        m_GameLevelMapView.OnClickGameLevelItem = OnClickGameLevelMapViewViewDetailsItem;
        DataTransfer data = new DataTransfer();
        ChapterEntity entity = ChapterDBModel.Instance.Get(1);
        data.SetData(ConstDefine.ChapterId, entity.Id);
        data.SetData(ConstDefine.ChapterName, entity.ChapterName);
        data.SetData(ConstDefine.ChapterBG, entity.BG_Pic);
        List<GameLevelEntity> entityList = GameLevelDBModel.Instance.GetNeedEntityById(entity.Id);
        if (entityList != null || entityList.Count > 0)
        {
            List<DataTransfer> dataArr = new List<DataTransfer>();
            for (int i = 0; i < entityList.Count; i++)
            {
                DataTransfer dataChild = new DataTransfer();
                dataArr.Add(dataChild);
                dataArr[i].SetData(ConstDefine.GameLevelId, entityList[i].Id);
                dataArr[i].SetData(ConstDefine.GameLevelIsBoss, entityList[i].isBoss);
                dataArr[i].SetData(ConstDefine.GameLevelName, entityList[i].Name);
                dataArr[i].SetData(ConstDefine.GameLevelPosInMap, entityList[i].GetPosInMap());
                dataArr[i].SetData(ConstDefine.GameLevelIcon, entityList[i].Ico);

            }
            data.SetData(ConstDefine.GameLevelArr, dataArr);
        }

        m_GameLevelMapView.SetUI(data);
    }
    #region 失败界面相关
    /// <summary>
    /// 打开关卡失败页面
    /// </summary>
    private void OpenGameLevelFail()
    {
        m_GameLevelFailView = UIViewUtil.Instance.OpenWindow(UIViewType.GameLevelFail).GetComponent<UIGameLevelFailView>();
        m_GameLevelFailView.OnbtnResurrection = OnClickGameLevelFailViewResurrectionButton;
        m_GameLevelFailView.OnbtnReturnWroldScene = OnClickGameLevelFailViewReturnWorldSceneButton;
        GameLevel_FailProto proto = new GameLevel_FailProto();
        proto.GameLevelId = m_GameLevelSceneId;
        proto.Grade = (byte)m_Garade;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());

    }
    /// <summary>
    /// 游戏关卡失败消息回调
    /// </summary>
    /// <param name="p"></param>
    private void GameLevel_FailReturnProtoCallBack(byte[] p)
    {
        GameLevel_FailReturnProto proto = GameLevel_FailReturnProto.ToProto(p);
        if (proto.IsSucess)
        {
            Debug.Log("GameLevel_FailReturnProto IsSucess");
        }
    }
    /// <summary>
    /// 点击关卡失败复活主城按钮
    /// </summary>
    private void OnClickGameLevelFailViewResurrectionButton()
    {
        GameLevel_ResurgenceProto proto = new GameLevel_ResurgenceProto();
        proto.GameLevelId = m_GameLevelSceneId;
        proto.Grade = (byte)m_Garade;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
        m_GameLevelFailView.Close();
    }


    private void GameLevel_ResurgenceReturnProtoCallBack(byte[] p)
    {
        GameLevel_ResurgenceReturnProto proto = GameLevel_ResurgenceReturnProto.ToProto(p);
        if (proto.IsSucess)
        {
            Global.Instance.CurPlayer.ToResurrection();
        }

    }
    /// <summary>
    /// 点击关卡失败返回主城按钮
    /// </summary>
    private void OnClickGameLevelFailViewReturnWorldSceneButton()
    {
        Global.Instance.CurPlayer.ToIdle(IdleType.IdleNormal);
        SceneMgr.Instance.LoadWorldMap(PlayerCtrl.Instance.LastWorldMapId);

    }
    #endregion
    #region 胜利界面相关
    /// <summary>
    /// 打开关卡胜利页面
    /// </summary>
    private void OpenGameLevelVictoryView()
    {
        m_GameLevelVictoryView = UIViewUtil.Instance.OpenWindow(UIViewType.GameLevelVictory).GetComponent<UIGameLevelVictoryView>();
        m_GameLevelVictoryView.OnReturnWorldScene = OnClickGameLevelVictoryViewReturnWorldSceneButton;
        GameLevelEntity levelEnitiy = GameLevelDBModel.Instance.Get(m_GameLevelSceneId);
        GameLevelGradeEntity gradeEnitiy = GameLevelGradeDBModel.Instance.GetEnityByLevelIdAndGrade(m_GameLevelSceneId, m_Garade);
        if (levelEnitiy == null || gradeEnitiy == null) return;
        DataTransfer data = new DataTransfer();
        data.SetData(ConstDefine.GameLevelRewardGold, gradeEnitiy.Gold);
        data.SetData(ConstDefine.GameLevelRewardExp, gradeEnitiy.Exp);
        data.SetData(ConstDefine.GameLevelPassTime, CurPassTime);
        int star = 1;
        if (CurPassTime < gradeEnitiy.Star2)
        {
            star = 3;
        }
        else if (CurPassTime < gradeEnitiy.Star1)
        {
            star = 2;
        }
        data.SetData(ConstDefine.GameLevelStarCount, star);

        List<GoodsEntity> rewards = new List<GoodsEntity>();

        //可以放在服务器里面计算，为了简单就直接在这计算了
        //装备
        rewards.Clear();
        int ranmdom01 = UnityEngine.Random.Range(0, 100);
        DataTransfer equipData = new DataTransfer();
        List<GoodsEntity> equipEnitys = gradeEnitiy.RewardEquipList;
        for (int i = 0; i < equipEnitys.Count; i++)
        {
            if (ranmdom01 < equipEnitys[i].GoodsProbability)
            {
                rewards.Add(equipEnitys[i]);
            }
        }
        GoodsEntity equipEnity = equipEnitys[UnityEngine.Random.Range(0, rewards.Count)];
        if (equipEnity != null)
        {
            equipData.SetData(ConstDefine.GameLevelGoodsId, equipEnity.GoodsId);
            equipData.SetData(ConstDefine.GameLevelGoodsName, equipEnity.GoodsName);
            equipData.SetData(ConstDefine.GameLevelGoodsProbability, equipEnity.GoodsProbability);
            equipData.SetData(ConstDefine.GameLevelGoodsCount, equipEnity.GoodsCount);
            data.SetData(ConstDefine.GameLevelRewardEquip, equipData);
            CurGameLevelGetGoodsList.Add(new GetGoodsEntity() { GoodsId = equipEnity.GoodsId, GoodsCount = 1, GoodsType = 0 });
        }


        //道具
        rewards.Clear();
        int ranmdom02 = UnityEngine.Random.Range(0, 100);
        DataTransfer itemData = new DataTransfer();
        List<GoodsEntity> itemEnitys = gradeEnitiy.RewardItemList;
        for (int i = 0; i < itemEnitys.Count; i++)
        {
            if (ranmdom02 < itemEnitys[i].GoodsProbability)
            {
                rewards.Add(itemEnitys[i]);
            }
        }
        GoodsEntity itemEnity = itemEnitys[UnityEngine.Random.Range(0, rewards.Count)];
        if (itemEnity != null)
        {
            itemData.SetData(ConstDefine.GameLevelGoodsId, itemEnity.GoodsId);
            itemData.SetData(ConstDefine.GameLevelGoodsName, itemEnity.GoodsName);
            itemData.SetData(ConstDefine.GameLevelGoodsProbability, itemEnity.GoodsProbability);
            itemData.SetData(ConstDefine.GameLevelGoodsCount, itemEnity.GoodsCount);
            data.SetData(ConstDefine.GameLevelRewardItem, itemData);
            CurGameLevelGetGoodsList.Add(new GetGoodsEntity() { GoodsId = itemEnity.GoodsId, GoodsCount = 1, GoodsType = 1 });
        }
        //材料
        rewards.Clear();
        int ranmdom03 = UnityEngine.Random.Range(0, 100);
        DataTransfer materiaData = new DataTransfer();
        List<GoodsEntity> materiaEnitys = gradeEnitiy.RewardMateriaList;
        for (int i = 0; i < materiaEnitys.Count; i++)
        {
            if (ranmdom03 < materiaEnitys[i].GoodsProbability)
            {
                rewards.Add(materiaEnitys[i]);
            }
        }
        GoodsEntity materiaEnity = materiaEnitys[UnityEngine.Random.Range(0, rewards.Count)];
        if (materiaEnity != null)
        {
            materiaData.SetData(ConstDefine.GameLevelGoodsId, materiaEnity.GoodsId);
            materiaData.SetData(ConstDefine.GameLevelGoodsName, materiaEnity.GoodsName);
            materiaData.SetData(ConstDefine.GameLevelGoodsProbability, materiaEnity.GoodsProbability);
            materiaData.SetData(ConstDefine.GameLevelGoodsCount, materiaEnity.GoodsCount);
            data.SetData(ConstDefine.GameLevelRewardMaterial, materiaData);
            CurGameLevelGetGoodsList.Add(new GetGoodsEntity() { GoodsId = materiaEnity.GoodsId, GoodsCount = 1, GoodsType = 2 });
        }
        m_GameLevelVictoryView.SetUI(data);

        CurGameLevelTotalExp += gradeEnitiy.Exp;
        CurGameLevelTotalGold += gradeEnitiy.Gold;
        GameLevel_VictoryProto proto = new GameLevel_VictoryProto();
        proto.GameLevelId = m_GameLevelSceneId;
        proto.Grade = (byte)m_Garade;
        proto.Star = (byte)star;
        proto.Exp = CurGameLevelTotalExp;
        proto.Gold = CurGameLevelTotalGold;
        foreach (var item in CurGameLevelKillMonsterDic)
        {
            GameLevel_VictoryProto.MonsterItem monster = new GameLevel_VictoryProto.MonsterItem();
            monster.MonsterId = item.Key;
            monster.MonsterCount = item.Value;
            proto.KillMonsterList.Add(monster);
        }
        foreach (var item in CurGameLevelGetGoodsList)
        {
            GameLevel_VictoryProto.ReceiveGoodsItem goods = new GameLevel_VictoryProto.ReceiveGoodsItem();
            goods.GoodsType = item.GoodsType;
            goods.GoodsId = item.GoodsId;
            goods.GoodsCount = item.GoodsCount;
            proto.ReceiveGoodsList.Add(goods);
        }
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }

    private void GameLevel_VictroyReturnProtoCallBack(byte[] p)
    {
        GameLevel_VictoryReturnProto proto = GameLevel_VictoryReturnProto.ToProto(p);
        if (proto.IsSucess)
        {
            Debug.Log("GameLevel_VictoryReturnProto IsSucess");
        }
    }
    #endregion
    #endregion

    #region 事件和委托

    #region 进入关卡
    /// <summary>
    /// 点击关卡详情页面进入关卡按钮
    /// </summary>
    /// <param name="p"></param>
    private void OnClickGameLevelDetailsViewEnterLevelButton(object[] p)
    {
        m_GameLevelSceneId = (int)p[0];
        m_Garade = (GameLevelGrade)p[1];
        GameLevel_EnterProto proto = new GameLevel_EnterProto();
        proto.GameLevelId = m_GameLevelSceneId;
        proto.Grade = (byte)m_Garade;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }

    private void GameLevel_EnterReturnProtoCallBack(byte[] p)
    {
        GameLevel_EnterReturnProto proto = GameLevel_EnterReturnProto.ToProto(p);
        if (proto.IsSucess)
        {
            SceneMgr.Instance.LoadGameLevel(m_GameLevelSceneId, m_Garade);
        }
    }
    #endregion
    /// <summary>
    /// 点击关卡详情难度按钮
    /// </summary>
    /// <param name="p"></param>
    private void OnClickGameLevelDetailsViewGradeButton(object[] p)
    {
        if (p.Length < 2)
        {
            return;
        }
        m_GameLevelSceneId = (int)p[0];
        m_Garade = (GameLevelGrade)p[1];
        SetGameLevelDetails(m_GameLevelSceneId, m_Garade);
    }
    /// <summary>
    ///  点击关卡胜利返回主城按钮
    /// </summary>
    private void OnClickGameLevelVictoryViewReturnWorldSceneButton()
    {
        SceneMgr.Instance.LoadWorldMap(PlayerCtrl.Instance.LastWorldMapId);
        Global.Instance.CurPlayer.ToIdle(IdleType.IdleNormal);
    }


    /// <summary>
    /// 点击关卡地图种的关卡详情
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickGameLevelMapViewViewDetailsItem(int obj)
    {
        m_GameLevelDetailsView = UIViewUtil.Instance.OpenWindow(UIViewType.GameLevelDetails).GetComponent<UIGameLevelDetailsView>();
        SetGameLevelDetails(obj, GameLevelGrade.Normal);

    }

    #endregion
    private void SetGameLevelDetails(int gameLevelId, GameLevelGrade grade)
    {
        m_GameLevelSceneId = gameLevelId;
        m_Garade = grade;
        GameLevelEntity levelEnitiy = GameLevelDBModel.Instance.Get(gameLevelId);
        GameLevelGradeEntity gradeEnitiy = GameLevelGradeDBModel.Instance.GetEnityByLevelIdAndGrade(gameLevelId, grade);
        if (levelEnitiy == null || gradeEnitiy == null) return;
        DataTransfer data = new DataTransfer();

        data.SetData(ConstDefine.GameLevelId, levelEnitiy.Id);
        data.SetData(ConstDefine.GameLevelName, levelEnitiy.Name);
        data.SetData(ConstDefine.GameLevelSceneName, levelEnitiy.SceneName);
        data.SetData(ConstDefine.GameLevelDlgPic, levelEnitiy.DlgPic);

        //装备
        List<DataTransfer> equipDatas = new List<DataTransfer>();
        List<GoodsEntity> equipEnitys = gradeEnitiy.RewardEquipList;

        if (equipEnitys != null && equipEnitys.Count > 0)
        {
            equipEnitys.Sort((GoodsEntity entity1, GoodsEntity entity2) =>
            {
                if (entity1.GoodsProbability > entity2.GoodsProbability)
                {
                    return 1;
                }
                else if (entity1.GoodsProbability == entity2.GoodsProbability)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            });
            for (int i = 0; i < equipEnitys.Count; i++)
            {
                DataTransfer equipData = new DataTransfer();
                equipData.SetData(ConstDefine.GameLevelGoodsId, equipEnitys[i].GoodsId);
                equipData.SetData(ConstDefine.GameLevelGoodsName, equipEnitys[i].GoodsName);
                equipData.SetData(ConstDefine.GameLevelGoodsProbability, equipEnitys[i].GoodsProbability);
                equipData.SetData(ConstDefine.GameLevelGoodsCount, equipEnitys[i].GoodsCount);
                equipDatas.Add(equipData);
            }

        }

        //道具
        List<DataTransfer> itemDatas = new List<DataTransfer>();
        List<GoodsEntity> itemEnitys = gradeEnitiy.RewardItemList;

        if (itemEnitys != null && itemEnitys.Count > 0)
        {
            itemEnitys.Sort((GoodsEntity entity1, GoodsEntity entity2) =>
            {
                if (entity1.GoodsProbability > entity2.GoodsProbability)
                {
                    return -1;
                }
                else if (entity1.GoodsProbability == entity2.GoodsProbability)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
            for (int i = 0; i < itemEnitys.Count; i++)
            {
                DataTransfer itemData = new DataTransfer();
                itemData.SetData(ConstDefine.GameLevelGoodsId, itemEnitys[i].GoodsId);
                itemData.SetData(ConstDefine.GameLevelGoodsName, itemEnitys[i].GoodsName);
                itemData.SetData(ConstDefine.GameLevelGoodsProbability, itemEnitys[i].GoodsProbability);
                itemData.SetData(ConstDefine.GameLevelGoodsCount, itemEnitys[i].GoodsCount);
                itemDatas.Add(itemData);
            }

        }
        //材料
        List<DataTransfer> materiaDatas = new List<DataTransfer>();
        List<GoodsEntity> materiaEnitys = gradeEnitiy.RewardMateriaList;

        if (materiaEnitys != null && materiaEnitys.Count > 0)
        {
            materiaEnitys.Sort((GoodsEntity entity1, GoodsEntity entity2) =>
            {
                if (entity1.GoodsProbability > entity2.GoodsProbability)
                {
                    return -1;
                }
                else if (entity1.GoodsProbability == entity2.GoodsProbability)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
            for (int i = 0; i < materiaEnitys.Count; i++)
            {
                DataTransfer materiaData = new DataTransfer();
                materiaData.SetData(ConstDefine.GameLevelGoodsId, materiaEnitys[i].GoodsId);
                materiaData.SetData(ConstDefine.GameLevelGoodsName, materiaEnitys[i].GoodsName);
                materiaData.SetData(ConstDefine.GameLevelGoodsProbability, materiaEnitys[i].GoodsProbability);
                materiaData.SetData(ConstDefine.GameLevelGoodsCount, materiaEnitys[i].GoodsCount);
                materiaDatas.Add(materiaData);
            }

        }

        data.SetData(ConstDefine.GameLevelRewardEquip, equipDatas);
        data.SetData(ConstDefine.GameLevelRewardItem, itemDatas);
        data.SetData(ConstDefine.GameLevelRewardMaterial, materiaDatas);

        data.SetData(ConstDefine.GameLevelRewardGold, gradeEnitiy.Gold);
        data.SetData(ConstDefine.GameLevelRewardExp, gradeEnitiy.Exp);
        data.SetData(ConstDefine.GameLevelDesc, gradeEnitiy.Desc);
        data.SetData(ConstDefine.GameLevelConditionDesc, gradeEnitiy.ConditionDesc);
        data.SetData(ConstDefine.GameLevelCommendFighting, gradeEnitiy.CommendFighting);
        m_GameLevelDetailsView.SetUI(data);
    }

}

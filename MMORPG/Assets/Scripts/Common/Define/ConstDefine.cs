using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ConstDefine : MonoBehaviour
{
    //快速登录
    public const string PlayerPrefs_AccountID_Key = "PlayerPrefs_AccountID_Key";
    public const string PlayerPrefs_UserName_Key = "PlayerPrefs_UserName_Key";
    public const string PlayerPrefs_Pwd_Key = "PlayerPrefs_Pwd_Key";

    //登录视图
    public const string UILogOnView_btnLogon = "UILogOnView_btnLogon";
    public const string UILogOnView_btnReg = "UILogOnView_btnReg";
    //注册视图
    public const string UIRegView_btnReg = "UIRegView_btnReg";
    public const string UIRegView_btnToLogon = "UIRegView_btnToLogon";

    //进入游戏视图
    public const string UIGameServerEnterView_btnGameServerSelect = "UIGameServerEnterView_btnGameServerSelect";
    public const string UIGameServerEnterView_btnGameServerEnter = "UIGameServerEnterView_btnGameServerEnter";

    //关卡详情视图
    public const string UIGameLevelDetailsView_Grade = "UIGameLevelDetailsView_Grade";
    public const string UIGameLevelDetailsView_EnterLevel = "UIGameLevelDetailsView_EnterLevel";

    //**************属性术语****************//

    public const string JobId = "JobId";
    public const string Level = "Level";
    public const string NickName = "NickName";
    public const string Exp = "Exp";
    public const string CurrentHP = "CurrentHP";
    public const string MaxHP = "MaxHP";
    public const string CurrentMP = "CurrentMP";
    public const string MaxMP = "MaxMP";
    public const string CurrentExp = "CurrentExp";
    public const string MaxExp = "MaxExp";
    public const string Attack = "Attack";
    public const string Defense = "Defense";
    public const string Hit = "Hit";
    public const string Res = "Res";
    public const string Dodge = "Dodge";
    public const string Cri = "Cri";
    public const string Fighting = "Fighting";
    public const string Gold = "Gold";
    public const string Money = "Money";


    //*************关卡地图相关***********************//
    //章
    public const string ChapterId = "ChapterId";
    public const string ChapterName = "ChapterName";
    public const string ChapterBG = "ChapterBG";
    //关卡
    public const string GameLevelArr = "GameLevelArr";

    public const string GameLevelId = "GameLevelId";
    public const string GameLevelIcon = "GameLevelIcon";
    public const string GameLevelName = "GameLevelName";
    public const string GameLevelSceneName = "GameLevelSceneName";
    public const string GameLevelDlgPic = "GameLevelDlgPic";
    public const string GameLevelPosInMap = "GameLevelPosInMap";
    public const string GameLevelIsBoss = "GameLevelIsBoss";

    public const string GameLevelRewardEquip = "GameLevelRewardEquip";
    public const string GameLevelRewardItem = "GameLevelRewardItem";
    public const string GameLevelRewardMaterial = "GameLevelRewardMaterial";
    public const string GameLevelRewardGold = "GameLevelRewardGold";
    public const string GameLevelRewardExp = "GameLevelRewardExp";
    public const string GameLevelDesc = "GameLevelRewardDesc";
    public const string GameLevelConditionDesc = "GameLevelConditionDesc";
    public const string GameLevelCommendFighting = "GameLevelCommendFighting";
    public const string GameLevelPassTime = "GameLevelPassTime";
    public const string GameLevelStarCount = "GameLevelStarCount";

    public const string GameLevelGoodsId = "GameLevelGoodsId";
    public const string GameLevelGoodsName = "GameLevelGoodsName";
    public const string GameLevelGoodsProbability = "GameLevelGoodsProbability";
    public const string GameLevelGoodsCount = "GameLevelGoodsCount";

    //----------------技能相关--------------------------------//
    public const string RoleInfoSkillID = "RoleInfoSkillID";
    public const string RoleInfoSkillLevel = "RoleInfoSkillLevel";
    public const string RoleInfoSkillPic = "RoleInfoSkillPic";
    public const string RoleInfoSlotsNO = "RoleInfoSlotsNO";
    public const string RoleInfoSkillCDTime = "RoleInfoSkillCDTime";
}

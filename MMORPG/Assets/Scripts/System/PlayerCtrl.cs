using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerCtrl : SystemCtrlBase<PlayerCtrl>, ISystemCtrl
{
    private UIRoleInfoView roleInfoView;
    private RoleInfoMainPlayer info;
    /// <summary>
    /// 最后进入的世界地图的场景ID
    /// </summary>
    public int LastWorldMapId;
    public void OpenView(UIViewType type)
    {
        switch (type)
        {
            case UIViewType.RoleInfo:
                OpenRoleInfo();
                break;
        }

    }

    /// <summary>
    /// 打开角色信息面板
    /// </summary>
    private void OpenRoleInfo()
    {
        roleInfoView = UIViewUtil.Instance.OpenWindow(UIViewType.RoleInfo).GetComponent<UIRoleInfoView>();
        DataTransfer data = new DataTransfer();
        data.SetData(ConstDefine.JobId, info.JobId);
        data.SetData(ConstDefine.NickName, info.RoleNickName);
        data.SetData(ConstDefine.Level, info.Level);
        data.SetData(ConstDefine.Fighting, info.Fighting);
        data.SetData(ConstDefine.Gold, info.Gold);
        data.SetData(ConstDefine.Money, info.Money);
        data.SetData(ConstDefine.CurrentHP, info.CurrentHP);
        data.SetData(ConstDefine.MaxHP, info.MaxHP);
        data.SetData(ConstDefine.CurrentMP, info.CurrentMP);
        data.SetData(ConstDefine.MaxMP, info.MaxMP);
        data.SetData(ConstDefine.CurrentExp, info.Exp);
        data.SetData(ConstDefine.MaxExp, 1000);
        data.SetData(ConstDefine.Attack, info.Attack);
        data.SetData(ConstDefine.Defense, info.Defense);
        data.SetData(ConstDefine.Dodge, info.Dodge);
        data.SetData(ConstDefine.Hit, info.Hit);
        data.SetData(ConstDefine.Res, info.Res);
        data.SetData(ConstDefine.Cri, info.Cri);
        roleInfoView.SetUI(data);
    }

    public void SetMainCityData()
    {
        SetMainCityPlayerInfo();
        SetMainCitySkillInfo();
    }

    private void SetMainCityPlayerInfo()
    {
        info = Global.Instance.CurRoleInfo;
        UIPlayerHeadBarInfoView.Instance.SetUI(info.RoleNickName, info.JobId, 1, info.Gold, info.Money, info.CurrentHP, info.MaxHP, info.CurrentMP, info.MaxMP);
        Global.Instance.CurPlayer.OnHPChange = OnHPChangeCallBack;
        Global.Instance.CurPlayer.OnMPChange = OnMPChangeCallBack;

    }

    private void OnHPChangeCallBack(ValueChangeType type)
    {
        if (info == null) return;
        UIPlayerHeadBarInfoView.Instance.SetHP(info.CurrentHP, info.MaxHP);
    }

    private void OnMPChangeCallBack(ValueChangeType type)
    {
        if (info == null) return;
        UIPlayerHeadBarInfoView.Instance.SetMP(info.CurrentMP, info.MaxMP);
    }

    private void SetMainCitySkillInfo()
    {
        List<DataTransfer> lst1 = new List<DataTransfer>();
        List<RoleInfoSkill> lst2 = Global.Instance.CurRoleInfo.SkillList;
        for (int i = 0; i < lst2.Count; i++)
        {
            DataTransfer dt = new DataTransfer();
            dt.SetData(ConstDefine.RoleInfoSlotsNO, lst2[i].SlotsNO);
            dt.SetData(ConstDefine.RoleInfoSkillID, lst2[i].SkillId);
            SkillEntity skill = SkillDBModel.Instance.Get(lst2[i].SkillId);
            if (skill != null)
            {
                dt.SetData(ConstDefine.RoleInfoSkillPic, skill.SkillPic);

            }
            SkillLevelEntity levelEntity = SkillLevelDBModel.Instance.GetEnityBySkillIdAndSkillLevel(lst2[i].SkillId, lst2[i].SKillLevel);
            if (levelEntity != null)
            {
                dt.SetData(ConstDefine.RoleInfoSkillCDTime, levelEntity.SkillCDTime);
            }
            lst1.Add(dt);
        }
        UIMainCitySkillView.Instance.SetUI(lst1);
        UIMainCitySkillView.Instance.OnClickSkillButton = ClickSkillButton;

    }

    public void ClickSkillButton(int skillId)
    {
        bool isSuccess = Global.Instance.CurPlayer.ToAttack(AttackType.SkillAttack, skillId);
        if (isSuccess)
        {
            UIMainCitySkillView.Instance.BeginCD(skillId);
            ((RoleInfoMainPlayer)Global.Instance.CurRoleInfo).SetSkillCDEndTime(skillId);
        }

;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCitySkillView : UISubViewBase
{
    public static UIMainCitySkillView Instance;

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
    }

    [SerializeField]
    private UIMainCitySkillSlotsNOView m_BtnSKill1;
    [SerializeField]
    private UIMainCitySkillSlotsNOView m_BtnSKill2;
    [SerializeField]
    private UIMainCitySkillSlotsNOView m_BtnSKill3;
    [SerializeField]
    private UIMainCitySkillSlotsNOView m_BtnAddHP;

    public Action<int> OnClickSkillButton;

    private Dictionary<int, UIMainCitySkillSlotsNOView> m_Dic = new Dictionary<int, UIMainCitySkillSlotsNOView>();
    protected override void OnStart()
    {
        base.OnStart();
        m_BtnSKill1.OnClickSkillButton = ClickSkillButton;
        m_BtnSKill2.OnClickSkillButton = ClickSkillButton;
        m_BtnSKill3.OnClickSkillButton = ClickSkillButton;
        m_BtnAddHP.OnClickSkillButton = ClickSkillButton;
    }

    private void ClickSkillButton(int obj)
    {
        if (OnClickSkillButton != null)
        {
            OnClickSkillButton(obj);
        }
    }

    public void SetUI(List<DataTransfer> datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            DataTransfer dt = datas[i];
            byte slotsNO = dt.GetData<byte>(ConstDefine.RoleInfoSlotsNO);
            int skillId = dt.GetData<int>(ConstDefine.RoleInfoSkillID);
            int SkillLevel = dt.GetData<int>(ConstDefine.RoleInfoSkillLevel);
            string skillPic = dt.GetData<string>(ConstDefine.RoleInfoSkillPic);
            float skllCD = dt.GetData<float>(ConstDefine.RoleInfoSkillCDTime);
            switch (slotsNO)
            {
                case 1:
                    m_BtnSKill1.SetUI(skillId, skllCD, skillPic);
                    m_Dic[skillId] = m_BtnSKill1;
                    break;
                case 2:
                    m_BtnSKill2.SetUI(skillId, skllCD, skillPic);
                    m_Dic[skillId] = m_BtnSKill2;
                    break;
                case 3:
                    m_BtnSKill3.SetUI(skillId, skllCD, skillPic);
                    m_Dic[skillId] = m_BtnSKill3;
                    break;
                default:
                    break;
            }
        }
    }

    public void BeginCD(int skillId)
    {
        foreach (var item in m_Dic)
        {
            if (item.Key == skillId)
            {
                item.Value.BeginCD();
            }
        }
    }
    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        OnClickSkillButton = null;
        m_BtnSKill1 = null;
        m_BtnSKill2 = null;
        m_BtnSKill3 = null;
    }
}

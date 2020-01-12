using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLevelVictoryView : UIWindowViewBase
{
    [SerializeField]
    private Text m_PassTime;
    [SerializeField]
    private Text m_RewardGold;
    [SerializeField]
    private Text m_RewardExp;
    [SerializeField]
    private UIGameLevelRewardItemView[] m_RewardItems;
    [SerializeField]
    private Transform[] m_StarArr;

    public Action OnReturnWorldScene;
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (go.name == "btnReturnWroldScene")
        {
            if (OnReturnWorldScene != null)
            {
                OnReturnWorldScene();
            }
        }
    }

    public void SetUI(DataTransfer data)
    {
        m_RewardGold.text = data.GetData<int>(ConstDefine.GameLevelRewardGold).ToString();
        m_RewardExp.text = data.GetData<int>(ConstDefine.GameLevelRewardExp).ToString();
        int passTime = data.GetData<int>(ConstDefine.GameLevelPassTime);
        m_PassTime.text = string.Format("通关时间：<color=#ff0000>{0}秒</color>", passTime);
        //装备
        DataTransfer rewardEquip = data.GetData<DataTransfer>(ConstDefine.GameLevelRewardEquip);
     
        if (rewardEquip != null)
        {
            m_RewardItems[0].gameObject.SetActive(true);
            m_RewardItems[0].SetUI(rewardEquip.GetData<int>(ConstDefine.GameLevelGoodsId), rewardEquip.GetData<string>(ConstDefine.GameLevelGoodsName), GameLevelRewardType.Equip);
        }
        else
        {
            m_RewardItems[0].gameObject.SetActive(false);
        }
        //道具
        DataTransfer rewardItem = data.GetData<DataTransfer>(ConstDefine.GameLevelRewardItem);
        if (rewardItem != null)
        {
            m_RewardItems[1].gameObject.SetActive(true);
            m_RewardItems[1].SetUI(rewardItem.GetData<int>(ConstDefine.GameLevelGoodsId), rewardItem.GetData<string>(ConstDefine.GameLevelGoodsName), GameLevelRewardType.Item);
        }
        else
        {
            m_RewardItems[1].gameObject.SetActive(false);
        }
        DataTransfer rewardMateria = data.GetData<DataTransfer>(ConstDefine.GameLevelRewardMaterial);
        if (rewardMateria != null)
        {
            m_RewardItems[2].gameObject.SetActive(true);
            m_RewardItems[2].SetUI(rewardMateria.GetData<int>(ConstDefine.GameLevelGoodsId), rewardMateria.GetData<string>(ConstDefine.GameLevelGoodsName), GameLevelRewardType.Material);
        }
        else
        {
            m_RewardItems[2].gameObject.SetActive(false);
        }

        int starCount = data.GetData<int>(ConstDefine.GameLevelStarCount);
        for (int i = 0; i < m_StarArr.Length; i++)
        {
            if (i >= starCount)
            {
                m_StarArr[i].gameObject.SetActive(false);
            }
            else
            {
                m_StarArr[i].gameObject.SetActive(true);
            }
        }
    }

}

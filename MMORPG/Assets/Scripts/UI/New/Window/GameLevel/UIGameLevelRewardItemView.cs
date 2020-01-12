using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLevelRewardItemView : UISubViewBase
{
    [SerializeField]
    private Text m_RewardName;
    [SerializeField]
    private Image m_Icon;

    public void SetUI(int id, string name, GameLevelRewardType rewardType)
    {
        m_Icon.sprite = GameUtil.LoadGameIcon(id.ToString(), rewardType);
        m_RewardName.text = name;
    }
    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        m_RewardName = null;
        m_Icon = null;
    }
}

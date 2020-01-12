using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLevelDetailsView : UIWindowViewBase
{

    [SerializeField]
    private Image m_DlgImage;
    [SerializeField]
    private Image[] m_GardeButtonImage;
    [SerializeField]
    private UIGameLevelRewardItemView[] m_RewardItems;
    [SerializeField]
    private Text m_RewardGold;
    [SerializeField]
    private Text m_RewardExp;
    [SerializeField]
    private Text m_DescText;
    [SerializeField]
    private Text m_ConditionText;
    [SerializeField]
    private Text m_FightingText;
    [SerializeField]
    private Text m_LevelName;
    [SerializeField]
    private Color m_SelectedColor;
    private int m_GameLevelId;
    private string m_GameLevelScene;
    private GameLevelGrade m_Grade;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnNormal":
                OnClickGradeButton(GameLevelGrade.Normal);
                break;
            case "btnHard":
                OnClickGradeButton(GameLevelGrade.Hard);
                break;
            case "btnHell":
                OnClickGradeButton(GameLevelGrade.Hell);
                break;
            case "btnEnterLevel":
                OnClickEnterLevelButton();
                break;
            default:
                break;
        }
    }

    private void OnClickEnterLevelButton()
    {
        object[] ps = new object[2];
        ps[0] = m_GameLevelId;
        ps[1] = m_Grade;
        UIDispatcher.Instance.Dispatc(ConstDefine.UIGameLevelDetailsView_EnterLevel, ps);
    }
    private void OnClickGradeButton(GameLevelGrade gradeType)
    {
        if (m_Grade == gradeType) return;
        m_Grade = gradeType;
        if (m_GardeButtonImage.Length == 3)
        {
            for (int i = 0; i < m_GardeButtonImage.Length; i++)
            {
                m_GardeButtonImage[i].color = Color.white;
            }
            m_GardeButtonImage[(int)gradeType].color = m_SelectedColor;
        }

        object[] ps1 = new object[2];
        ps1[0] = m_GameLevelId;
        ps1[1] = gradeType;
        UIDispatcher.Instance.Dispatc(ConstDefine.UIGameLevelDetailsView_Grade, ps1);
    }
    public void SetUI(DataTransfer data)
    {
        m_GameLevelId = data.GetData<int>(ConstDefine.GameLevelId);
        m_LevelName.text = data.GetData<string>(ConstDefine.GameLevelName);
        m_GameLevelScene = data.GetData<string>(ConstDefine.GameLevelSceneName);
        string dlgPicStr = data.GetData<string>(ConstDefine.GameLevelDlgPic);
        m_DlgImage.sprite = GameUtil.LoadGameLevelDlgBG(dlgPicStr);
        m_RewardGold.text = data.GetData<int>(ConstDefine.GameLevelRewardGold).ToString();
        m_RewardExp.text = data.GetData<int>(ConstDefine.GameLevelRewardExp).ToString();
        m_DescText.text = data.GetData<string>(ConstDefine.GameLevelDesc);
        m_ConditionText.text = data.GetData<string>(ConstDefine.GameLevelConditionDesc);
        m_FightingText.text = data.GetData<int>(ConstDefine.GameLevelCommendFighting).ToString();
        for (int i = 0; i < m_RewardItems.Length; i++)
        {
            m_RewardItems[i].gameObject.SetActive(false);
        }
        //装备
        List<DataTransfer> rewardEquipList = data.GetData<List<DataTransfer>>(ConstDefine.GameLevelRewardEquip);
        if (rewardEquipList != null && rewardEquipList.Count > 0)
        {
            m_RewardItems[0].gameObject.SetActive(true);
            m_RewardItems[0].SetUI(rewardEquipList[0].GetData<int>(ConstDefine.GameLevelGoodsId), rewardEquipList[0].GetData<string>(ConstDefine.GameLevelGoodsName), GameLevelRewardType.Equip);
        }
        else
        {
            m_RewardItems[0].gameObject.SetActive(false);
        }
        //道具
        List<DataTransfer> rewardItemList = data.GetData<List<DataTransfer>>(ConstDefine.GameLevelRewardItem);
        if (rewardItemList != null && rewardItemList.Count > 0)
        {
            m_RewardItems[1].SetUI(rewardItemList[0].GetData<int>(ConstDefine.GameLevelGoodsId), rewardItemList[0].GetData<string>(ConstDefine.GameLevelGoodsName), GameLevelRewardType.Item);
            m_RewardItems[1].gameObject.SetActive(true);
        }
        else
        {
            m_RewardItems[1].gameObject.SetActive(false);
        }
        //材料
        List<DataTransfer> rewardMaterialList = data.GetData<List<DataTransfer>>(ConstDefine.GameLevelRewardMaterial);
        if (rewardMaterialList != null && rewardMaterialList.Count > 0)
        {
            m_RewardItems[2].gameObject.SetActive(true);
            m_RewardItems[2].SetUI(rewardMaterialList[0].GetData<int>(ConstDefine.GameLevelGoodsId), rewardMaterialList[0].GetData<string>(ConstDefine.GameLevelGoodsName), GameLevelRewardType.Material);
        }
        else
        {
            m_RewardItems[2].gameObject.SetActive(false);
        }
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        m_DlgImage = null;
        m_FightingText = null;
        m_ConditionText = null;
        m_DescText = null;
        m_RewardExp = null;
        m_GardeButtonImage = null;
        m_LevelName = null;
    }


}

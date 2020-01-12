using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLevelItemView : UISubViewBase
{
    [SerializeField]
    private Image m_IconImage;
    [SerializeField]
    private Text m_NameIext;
    [SerializeField]
    private Button m_Button;

    private int m_LevelId;

    public Action<int> OnClickGameLevelItem;
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
        m_Button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        if (OnClickGameLevelItem != null)
        {
            OnClickGameLevelItem(m_LevelId);
        }
    }

    public void SetUI(DataTransfer data)
    {

        m_LevelId = data.GetData<int>(ConstDefine.GameLevelId);
        int isBoss = data.GetData<int>(ConstDefine.GameLevelIsBoss);
        string name = data.GetData<string>(ConstDefine.GameLevelName);
        string icon = data.GetData<string>(ConstDefine.GameLevelIcon);
        m_IconImage.sprite = GameUtil.LoadGameLevelItemBG(icon);
        m_NameIext.text = name;
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
    }
}

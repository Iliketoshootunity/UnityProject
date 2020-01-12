using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleDataView : UISubViewBase
{

    [SerializeField]
    private Text m_GlodText;
    [SerializeField]
    private Text m_MoneyText;
    [SerializeField]
    private Text m_AttackText;
    [SerializeField]
    private Text m_DefenseText;
    [SerializeField]
    private Text m_HitText;
    [SerializeField]
    private Text m_DodgeText;
    [SerializeField]
    private Text m_ResText;
    [SerializeField]
    private Text m_CriText;
    [SerializeField]
    private Text m_HPText;
    [SerializeField]
    private Text m_MPText;
    [SerializeField]
    private Text m_ExpText;
    [SerializeField]
    private Slider m_HPSlider;
    [SerializeField]
    private Slider m_MPSlider;
    [SerializeField]
    private Slider m_ExpSlider;

    public void SetUI(DataTransfer data)
    {
        int glod = data.GetData<int>(ConstDefine.Gold);
        int money = data.GetData<int>(ConstDefine.Money);
        int curHp = data.GetData<int>(ConstDefine.CurrentHP);
        int maxHp = data.GetData<int>(ConstDefine.MaxHP);
        int curMP = data.GetData<int>(ConstDefine.CurrentMP);
        int maxMp = data.GetData<int>(ConstDefine.MaxMP);
        int curExp = data.GetData<int>(ConstDefine.CurrentExp);
        int maxExp = data.GetData<int>(ConstDefine.MaxExp);
        int attack = data.GetData<int>(ConstDefine.Attack);
        int defense = data.GetData<int>(ConstDefine.Defense);
        int dogge = data.GetData<int>(ConstDefine.Dodge);
        int hit = data.GetData<int>(ConstDefine.Hit);
        int res = data.GetData<int>(ConstDefine.Res);
        int cri = data.GetData<int>(ConstDefine.Cri);

        m_GlodText.text = glod.ToString();
        m_MoneyText.text = money.ToString();
        m_AttackText.text = attack.ToString();
        m_DefenseText.text = defense.ToString();
        m_HitText.text = hit.ToString();
        m_DodgeText.text = dogge.ToString();
        m_ResText.text = res.ToString();
        m_CriText.text = cri.ToString();
        m_HPText.text = string.Format("{0}/{1}", curHp, maxHp);
        m_MPText.text = string.Format("{0}/{1}", curMP, maxMp);
        m_ExpText.text = string.Format("{0}/{1}", curExp, maxExp);

        m_HPSlider.value = (float)(curHp / (float)maxHp);
        m_MPSlider.value = (float)(curMP / (float)maxMp);
        m_ExpSlider.value = (float)(curExp / (float)maxExp);
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        m_GlodText = null;
        m_MoneyText = null;
        m_AttackText = null;
        m_DefenseText = null;
        m_HitText = null;
        m_DodgeText = null;
        m_ResText = null;
        m_CriText = null;
        m_HPText = null;
        m_MPText = null;
        m_ExpText = null;
        m_HPSlider = null;
        m_MPSlider = null;
        m_ExpSlider = null;
    }
}

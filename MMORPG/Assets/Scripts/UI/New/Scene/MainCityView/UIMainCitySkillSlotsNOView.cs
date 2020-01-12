using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCitySkillSlotsNOView : UISubViewBase
{
    /// <summary>
    /// 技能槽Id
    /// </summary>
    public int SlotsNOId;

    /// <summary>
    /// 技能ID
    /// </summary>
    private int m_SkillId;

    public Image SkillImage;
    public Image CDmage;

    private float m_BeginCDTime;
    private float m_CDTime;
    private bool m_IsCD = false;
    public int SkillId { get { return m_SkillId; } }

    public Action<int> OnClickSkillButton;

    protected override void OnStart()
    {
        base.OnStart();
        EventTriggerListener.Get(gameObject).onClick += onClick;
        CDmage.gameObject.SetActive(false);
    }

    private void onClick(GameObject go)
    {
        if (SkillId < 0) return;
        if (m_IsCD) return;
        if (OnClickSkillButton != null)
        {
            OnClickSkillButton(SkillId);
        }
    }

    public void SetUI(int skillId, float skillCDTime, string skillPic)
    {
        m_SkillId = skillId;
        m_CDTime = skillCDTime;
        Sprite sprite = RoleMgr.Instance.LoadSkillPic(skillPic);
        if (sprite != null)
        {
            SkillImage.sprite = sprite;
        }
    }

    public void BeginCD()
    {
        CDmage.gameObject.SetActive(true);
        m_IsCD = true;
        m_BeginCDTime = Time.time;
    }

    private void Update()
    {
        if (m_IsCD)
        {
            float process = Mathf.Lerp(1, 0, (Time.time - m_BeginCDTime) / m_CDTime);
            CDmage.fillAmount = process;
            if (Time.time > m_BeginCDTime + m_CDTime)
            {
                m_IsCD = false;
            }
        }
    }
}

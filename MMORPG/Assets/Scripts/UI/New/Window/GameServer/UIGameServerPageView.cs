using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameServerPageView : MonoBehaviour
{

    /// <summary>
    /// Ò³Ç©
    /// </summary>
    private int m_PageIndex;

    /// <summary>
    /// Ãû×Ö
    /// </summary>
    [SerializeField]
    private Text m_NameText;

    /// <summary>
    /// Ãû×Ö
    /// </summary>
    [SerializeField]
    private Button button;

    public Action<int> OnClickGameServerPage;

    private void Start()
    {
        EventTriggerListener.Get(button.gameObject).onClick += ClickButton;
    }
    private void OnDestroy()
    {
        m_NameText = null;
        button = null;
        OnClickGameServerPage = null;
    }
    public void SetUI(RetGameServerPageEntity entity)
    {
        m_PageIndex = entity.PageIndex;
        m_NameText.text = entity.Name;

    }

    private void ClickButton(GameObject go)
    {
        if (OnClickGameServerPage != null)
            OnClickGameServerPage(m_PageIndex);
    }
}

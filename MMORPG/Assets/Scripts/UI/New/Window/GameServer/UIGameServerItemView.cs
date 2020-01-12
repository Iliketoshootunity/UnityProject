using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIGameServerItemView : MonoBehaviour
{
    [SerializeField]
    private Color[] m_StatusColors;

    [SerializeField]
    private Image m_CurrentStatusImage;

    [SerializeField]
    private Text m_NameText;
    [SerializeField]
    private Button m_Button;

    private RetGameserverEntity m_RetData;


    public Action<RetGameserverEntity> OnClickGameServerItem;

    // Use this for initialization
    void Start()
    {
        EventTriggerListener.Get(m_Button.gameObject).onClick += OnClick;
    }

    private void OnClick(GameObject go)
    {
       if(OnClickGameServerItem!=null)
        {
            OnClickGameServerItem(m_RetData);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        m_CurrentStatusImage = null;
        m_NameText = null;
        m_Button = null;
        m_RetData = null;
        OnClickGameServerItem = null;
    }
    public void SetUI(RetGameserverEntity entity)
    {
        Color curColor = m_StatusColors[entity.RunSatus];
        m_CurrentStatusImage.color = curColor;
        m_NameText.text = entity.Name;
        m_RetData = entity;
    }
}

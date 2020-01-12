using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITipViewItem : UISubViewBase
{
    [SerializeField]
    private Text m_valueText;
    [SerializeField]
    private Image m_TypeImage;
    [SerializeField]
    private Sprite[] m_TypeSpriteArr;

    public void SetUI(TipEntity tipEntity)
    {
        m_valueText.text = tipEntity.Value.ToString();
        if (tipEntity.Type == 0)
        {
            m_TypeImage.sprite = m_TypeSpriteArr[0];
        }
        else if (tipEntity.Type == 1)
        {
            m_TypeImage.sprite = m_TypeSpriteArr[1];
        }
    }


}

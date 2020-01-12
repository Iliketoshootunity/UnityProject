using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLevelMapPointView : UISubViewBase
{
    [SerializeField]
    private Image m_PassImage;
    [SerializeField]
    private Image m_NoPassImage;

    public void SetUI(bool isPass)
    {
        m_PassImage.gameObject.SetActive(false);
        m_NoPassImage.gameObject.SetActive(false);
        if (isPass)
        {
            m_PassImage.gameObject.SetActive(true);
        }
        else
        {
            m_NoPassImage.gameObject.SetActive(true);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoleInfoView : UIWindowViewBase
{
    [SerializeField]
    private UIRoleEuqipmentView m_EuqipmentView;
    [SerializeField]
    private UIRoleDataView m_DataView;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
    }

    protected override void BeforeOnDestory()
    {
        base.BeforeOnDestory();
        m_EuqipmentView = null;
        m_DataView = null;
    }

    public void SetUI(DataTransfer data)
    {
        m_EuqipmentView.SetUI(data);
        m_DataView.SetUI(data);
    }
}

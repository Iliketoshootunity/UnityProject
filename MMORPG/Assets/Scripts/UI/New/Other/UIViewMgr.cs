using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewMgr : Singleton<UIViewMgr>
{

    private Dictionary<UIViewType, ISystemCtrl> m_Dic = new Dictionary<UIViewType, ISystemCtrl>();

    public UIViewMgr()
    {
        //��¼ҳ�� ���Ʋ����ͼ�� ��ӳ���ϵ
        m_Dic.Add(UIViewType.LogOn, AccountCtrl.Instance);
        m_Dic.Add(UIViewType.Reg, AccountCtrl.Instance);

        //������ҳ�� ���Ʋ����ͼ���ӳ���ϵ
        m_Dic.Add(UIViewType.GameServerEnter, GameServerCtrl.Instance);
        m_Dic.Add(UIViewType.GameServerSelect, GameServerCtrl.Instance);

        //��ɫҳ�� ���Ʋ����ͼ���ӳ���ϵ
        m_Dic.Add(UIViewType.RoleInfo, PlayerCtrl.Instance);

        //�ؿ�ҳ�� ���Ʋ����ͼ���ӳ���ϵ
        m_Dic.Add(UIViewType.GameLevelMap, GameLevelCtrl.Instance);
        m_Dic.Add(UIViewType.GameLevelVictory, GameLevelCtrl.Instance);
        m_Dic.Add(UIViewType.GameLevelFail, GameLevelCtrl.Instance);
    }

    public void OpenView(UIViewType type)
    {
        if (!m_Dic.ContainsKey(type)) return;
        m_Dic[type].OpenView(type);
    }
}

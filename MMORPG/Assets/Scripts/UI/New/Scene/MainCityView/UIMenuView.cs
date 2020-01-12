using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIMenuView : MonoBehaviour
{
    private Vector3 m_MoveTarget;
    private Action OnComplete;
    private bool m_IsShow;
    private bool m_IsBusy;

    public static UIMenuView Instance;
    private void Awake()
    {
        Instance = this;
        m_IsShow = true;
    }
    void Start()
    {
        m_MoveTarget = transform.localPosition + new Vector3(0, 100, 0);
        transform.DOLocalMove(m_MoveTarget, 0.2f).SetAutoKill(false).Pause().OnComplete(() =>
         {
             m_IsBusy = false;
             if (OnComplete != null) OnComplete();

         }).OnRewind(() =>
         {
             m_IsBusy = false;
             if (OnComplete != null) OnComplete();
         });

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeStatus(Action onChangeOk)
    {
        if (m_IsBusy) return;
        OnComplete = onChangeOk;
        m_IsShow = !m_IsShow;
        m_IsBusy = true;
        if (m_IsShow)
        {
            transform.DOPlayBackwards();
        }
        else
        {
            transform.DOPlayForward();

        }
    }
}

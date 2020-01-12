using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCHeadBar : MonoBehaviour
{


    [SerializeField]
    private Text m_NickNameText;
    [SerializeField]
    private Image m_TalkImage;
    [SerializeField]
    private Text m_TaklText;
    /// <summary>
    /// 对齐的目标点
    /// </summary>
    private GameObject target;

    private string m_TalkStr;
    private bool m_IsShow;
    private float m_ShowTime;
    private Tween m_ScaleTween;
    private Tween m_RotateTween;
    private void Start()
    {
        transform.localScale = Vector3.zero;
        m_ScaleTween = transform.DOScale(Vector3.one, 0.2f).SetAutoKill(false).SetEase(Ease.Linear).Pause().OnComplete(() =>
        {
            m_TaklText.text = "";
            m_TaklText.DOText(m_TalkStr, 0.5f);
        }).OnRewind(() =>
        {
            gameObject.SetActive(false);
        });
        transform.localEulerAngles = new Vector3(0, 0, 10);
        m_RotateTween = transform.DOLocalRotate(new Vector3(0, 0, -20f), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack).Pause().SetAutoKill(false).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        if (m_NickNameText == null || target == null || m_TalkImage == null) return;
        Vector3 scenePoint = Camera.main.WorldToScreenPoint(target.transform.position);
        Vector3 uiPoint = new Vector3(scenePoint.x, -(Screen.height - scenePoint.y), scenePoint.z);
        transform.localPosition = uiPoint;
        if (m_IsShow && Time.time > m_ShowTime)
        {
            m_ScaleTween.PlayBackwards();
            m_IsShow = false;
        }
    }
    public void Init(GameObject target, string name)
    {
        this.target = target;
        m_NickNameText.text = name;
    }

    public void InitTalk(string talk, float showTime)
    {
        m_TalkStr = talk;
        gameObject.SetActive(true);
        m_IsShow = true;
        m_ScaleTween.PlayForward();
        m_RotateTween.PlayForward();
        m_ShowTime = Time.time + showTime;
    }
    // Update is called once per frame

    private void OnDestroy()
    {
        m_NickNameText = null;
        target = null;
        m_TalkImage = null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerEvent : MonoBehaviour
{

    public enum FingerDir
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public enum ZoomType
    {
        None,
        In,
        Out
    }



    public System.Action<FingerDir> OnFingerDir;
    public System.Action OnPlayerClick;
    public System.Action<ZoomType> OnFingerZoom;
    public static FingerEvent Instance;

    private Vector2 oldFingerPos;
    private Vector2 oldFingerPos1;
    private Vector2 oldFingerPos2;

    //1.按下 2.开始拖拽 3. 拖拽中 4.拖拽结束
    private int fingerStepIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        FingerGestures.OnFingerDragMove += FingerDragMoveCallBack;
        FingerGestures.OnFingerDown += FingerDownCallBack;
        FingerGestures.OnFingerDragBegin += FingerDragBeginCallBack;
        FingerGestures.OnFingerDragEnd += FingerDragEndCallBack;
        FingerGestures.OnFingerUp += FingerUpCallBack;
    }

    private void Update()
    {
        ZoomCtrl();
    }

    private void ZoomCtrl()
    {
        if (OnFingerZoom == null) return;
        ZoomType zoomType = ZoomType.None;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            zoomType = ZoomType.In;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            zoomType = ZoomType.Out;
        }

#elif UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount == 2)
        {
            if(Vector2.Distance(oldFingerPos1, oldFingerPos2) > Vector2.Distance(oldFingerPos1, oldFingerPos2))
            {
                zoomType = ZoomType.Out;
            }
            else
            {
                zoomType = ZoomType.In;
            }
            oldFingerPos1 = Input.touches[0].position;
            oldFingerPos2 = Input.touches[1].position;
        }
#endif
        OnFingerZoom(zoomType);
    }

    private void FingerDragEndCallBack(int fingerIndex, Vector2 fingerPos)
    {
        fingerStepIndex = 4;
    }

    private void FingerUpCallBack(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
        if (fingerStepIndex == 1)
        {
            //Ray ray = UICamera.currentCamera.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("UI")))
            //{
            //    Debug.Log("ray UI");
            //    return;
            //}
            fingerStepIndex = -1;
            if (OnPlayerClick != null)
            {
                OnPlayerClick();
            }
        }
    }


    private void FingerDragBeginCallBack(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
    {
        oldFingerPos = fingerPos;
        fingerStepIndex = 2;
    }

    private void FingerDownCallBack(int fingerIndex, Vector2 fingerPos)
    {
        fingerStepIndex = 1;
    }

    private void FingerDragMoveCallBack(int fingerIndex, Vector2 fingerPos, Vector2 delta)
    {
        fingerStepIndex = 3;
        if (UIViewUtil.Instance.OpenWindowCount > 0) return;
        Vector3 dir = fingerPos - oldFingerPos;
        FingerDir fingerDir = FingerDir.None;
        //向上
        if (dir.y > -dir.x && dir.y > dir.x)
        {
            fingerDir = FingerDir.Up;
        }
        //向下
        if (-dir.y > -dir.x && -dir.y > dir.x)
        {
            fingerDir = FingerDir.Down;
        }
        //向左
        if (-dir.x > dir.y && -dir.x > -dir.y)
        {
            fingerDir = FingerDir.Left;
        }
        //向右
        if (dir.x > dir.y && dir.x > -dir.y)
        {
            fingerDir = FingerDir.Right;
        }

        if (OnFingerDir != null) OnFingerDir(fingerDir);


    }
}




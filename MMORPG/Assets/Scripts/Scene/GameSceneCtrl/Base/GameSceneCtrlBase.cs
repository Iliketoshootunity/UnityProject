using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 3d场景控制器基类
/// </summary>
public class GameSceneCtrlBase : MonoBehaviour
{
    protected UISceneMainCityView mainCityView;
    public Vector3 TestPoint;
    void Awake()
    {
        if (FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnFingerDir += OnFingerDir;
            FingerEvent.Instance.OnPlayerClick += OnPlayerClick;
            FingerEvent.Instance.OnFingerZoom += OnFingerZoom;

        }
        mainCityView = UISceneCtrl.Instance.Load(UISceneType.MainCity).GetComponent<UISceneMainCityView>();
        mainCityView.OnLoadComplete = OnMainUIComplete;
        OnAwake();
    }


    void Start()
    {
        OnStart();
        EffectMgr.Instance.Init(this);

    }

    void Update()
    {
        OnUpdate();
    }
    void OnDestroy()
    {
        if (FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnFingerDir -= OnFingerDir;
            FingerEvent.Instance.OnPlayerClick -= OnPlayerClick;
            FingerEvent.Instance.OnFingerZoom -= OnFingerZoom;
        }
        EffectMgr.Instance.Clear();
        BeforeOnDestory();
    }
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void BeforeOnDestory() { }
    protected virtual void OnMainUIComplete() { }


    #region 手指控制 委托回调

    /// <summary>
    /// 手指缩放
    /// </summary>
    /// <param name="obj"></param>
    private void OnFingerZoom(FingerEvent.ZoomType obj)
    {
        switch (obj)
        {
            case FingerEvent.ZoomType.In:
                CameraCtrl.Instance.SetCameraZoom(0);
                break;
            case FingerEvent.ZoomType.Out:
                CameraCtrl.Instance.SetCameraZoom(1);
                break;
        }

    }
    /// <summary>
    /// 手指点击地面
    /// </summary>
    private void OnPlayerClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Role"));
        if (hitArr.Length > 0)
        {
            for (int i = 0; i < hitArr.Length; i++)
            {
                RoleCtrl ctrl = hitArr[i].collider.GetComponent<RoleCtrl>();
                if (ctrl.CurRoleType == RoleType.MainPlayer)
                {
                    continue;
                }
                Global.Instance.CurPlayer.LockEnemy = ctrl;
                return;
            }
        }
        if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            RaycastHit hitinfo1;
            Ray ray1 = new Ray(new Vector3(hitinfo.point.x, hitinfo.point.y + 1000, hitinfo.point.z), -Vector3.up);
            if (Physics.Raycast(ray1, out hitinfo1, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
            {
                return;
            }
            Global.Instance.CurPlayer.LockEnemy = null;
            Global.Instance.CurPlayer.MoveTo(hitinfo.point);
        }

    }
    /// <summary>
    /// 手指滑动方向
    /// </summary>
    /// <param name="obj"></param>

    private void OnFingerDir(FingerEvent.FingerDir obj)
    {
        switch (obj)
        {
            case FingerEvent.FingerDir.Up:
                CameraCtrl.Instance.SetCameraUpAndDown(0);
                break;
            case FingerEvent.FingerDir.Down:
                CameraCtrl.Instance.SetCameraUpAndDown(1);
                break;
            case FingerEvent.FingerDir.Left:
                CameraCtrl.Instance.SetCameraRotate(0);
                break;
            case FingerEvent.FingerDir.Right:
                CameraCtrl.Instance.SetCameraRotate(1);
                break;
        }

    }
    #endregion


}


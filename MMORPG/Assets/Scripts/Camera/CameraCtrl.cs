using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    public static CameraCtrl Instance;

    [SerializeField]
    private Transform CameraRotate;
    [SerializeField]
    private Transform CameraUpAndDown;
    [SerializeField]
    private Transform CameraZoom;
    [SerializeField]
    private Transform CameraZoomContainer;
    void Awake()
    {
        Instance = this;
    }
	// Use this for initialization
	void Start () {
	
	}

    public void Init()
    {
        CameraUpAndDown.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(CameraUpAndDown.localEulerAngles.z, 25, 60));
        CameraZoom.localPosition = new Vector3(0, 0, -10);
    }

    public void SetCameraRotate(int type)
    {
        CameraRotate.Rotate(0, 60 * Time.deltaTime * (type == 0 ? 1 : -1),0);
    }

    public void SetCameraUpAndDown(int type)
    {
        CameraUpAndDown.Rotate(0, 0,60 * Time.deltaTime * (type == 0 ? 1 : -1));
        CameraUpAndDown.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(CameraUpAndDown.localEulerAngles.z, 25, 60));
    }

    public void SetCameraZoom(int type)
    {
        CameraZoom.Translate(Vector3.forward * Time.deltaTime * 10 * (type == 0 ? 1 : -1));
        CameraZoom.localPosition= new Vector3(0, 0, Mathf.Clamp(CameraZoom.localPosition.z, -20, 3));
    }

    public void AutoLookAt(Vector3 pos)
    {
        CameraZoomContainer.LookAt(pos);
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 15);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 14);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}

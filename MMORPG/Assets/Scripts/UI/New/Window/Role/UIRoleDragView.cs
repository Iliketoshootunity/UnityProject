using UnityEngine;
using UnityEngine.EventSystems;
public class UIRoleDragView : UISubViewBase, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField]
    private Transform m_RotateTarget;

    private Vector3 m_BeginPos;

    private Vector3 m_EndPos;

    [SerializeField]
    private float m_RotateSpeed;


    private void OnDestroy()
    {
        m_RotateTarget = null;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_BeginPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        m_EndPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_EndPos = eventData.position;
        m_RotateTarget.Rotate(0, (m_BeginPos.x > m_EndPos.x ? 1 : -1) * m_RotateSpeed * Time.deltaTime, 0);
        m_BeginPos = m_EndPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleHeadBar : MonoBehaviour
{

    [SerializeField]
    private Text nickName;
    [SerializeField]
    private Slider phHP;


    /// <summary>
    /// 对齐的目标点
    /// </summary>
    private GameObject target;

    public void Init(GameObject target, string name, bool isShowHp = true, float hpSliderValue = 1)
    {
        this.target = target;
        nickName.text = name;
        phHP.gameObject.SetActive(isShowHp);
        if (isShowHp)
        {
            phHP.value = hpSliderValue;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (nickName == null || target == null) return;
        Vector3 scenePoint = Camera.main.WorldToScreenPoint(target.transform.position);
        Vector3 uiPoint = new Vector3(scenePoint.x, -(Screen.height - scenePoint.y), scenePoint.z);
        transform.localPosition = uiPoint;
    }

    public void SetHpSlider(float hpSliderValue)
    {
        phHP.value = hpSliderValue;
    }

    private void OnDestroy()
    {
        nickName = null;
        nickName = null;
        phHP = null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIGameLevelMapView : UIWindowViewBase
{
    [SerializeField]
    private RawImage m_ChapterBG;

    [SerializeField]
    private Text m_ChapterName;

    [SerializeField]
    private Transform m_PointContainer;

    private List<Transform> mapItems = new List<Transform>();
    public int ChapterId;
    private DataTransfer data;
    public Action<int> OnClickGameLevelItem;

    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine("CreateItem");
    }
    private IEnumerator CreateItem()
    {
        if (data == null) yield break;
        ChapterId = data.GetData<int>(ConstDefine.ChapterId);
        m_ChapterName.text = data.GetData<string>(ConstDefine.ChapterName);
        string bgName = data.GetData<string>(ConstDefine.ChapterBG);
        m_ChapterBG.texture = GameUtil.LoadGameLevelMapBG(bgName);
        List<DataTransfer> dataArr = data.GetData<List<DataTransfer>>(ConstDefine.GameLevelArr);
        mapItems.Clear();
        for (int i = 0; i < dataArr.Count; i++)
        {
            GameObject go = ResourcesMrg.Instance.Load(ResourcesMrg.ResourceType.UIWindowChild, "GameLevelItem");
            Vector2 pos = dataArr[i].GetData<Vector2>(ConstDefine.GameLevelPosInMap);
            go.transform.SetParent(m_ChapterBG.transform);
            go.transform.localPosition = pos;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            UIGameLevelItemView itemView = go.GetComponent<UIGameLevelItemView>();
            itemView.OnClickGameLevelItem = ClickGameLevelItem;
            itemView.SetUI(dataArr[i]);
            mapItems.Add(go.transform);
            yield return null;
        }

        for (int i = 0; i < mapItems.Count; i++)
        {
            if (i == mapItems.Count - 1) break;
            Vector3 beginPos = mapItems[i].transform.localPosition;
            Vector3 endPos = mapItems[i + 1].transform.localPosition;
            float distance = Vector3.Distance(beginPos, endPos);
            int count = (int)(distance / 20);
            float xStep = (endPos.x - beginPos.x) / count;
            float yStep = (endPos.y - beginPos.y) / count;
            for (int j = 0; j < count; j++)
            {
                if (j == count - 1) break;
                GameObject go = ResourcesMrg.Instance.Load(ResourcesMrg.ResourceType.UIWindowChild, "GameLevelPointItem");
                go.transform.SetParent(m_PointContainer);
                go.transform.localPosition = new Vector3(beginPos.x + xStep * j, beginPos.y + yStep * j, 0);
                go.transform.localScale = Vector3.one;
                go.transform.rotation = Quaternion.identity;
                UIGameLevelMapPointView pointView = go.GetComponent<UIGameLevelMapPointView>();
                pointView.SetUI(false);
                yield return null;
            }
        }
    }

    public void SetUI(DataTransfer data)
    {
        this.data = data;


    }

    private void ClickGameLevelItem(int id)
    {
        if (OnClickGameLevelItem != null)
        {
            OnClickGameLevelItem(id);
        }
    }

}

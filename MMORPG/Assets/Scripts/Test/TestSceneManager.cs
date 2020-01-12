using UnityEngine;
using System.Collections;

public class TestSceneManager : MonoBehaviour
{

    [SerializeField]
    private Transform boxCreateArea;
    [SerializeField]
    private Transform boxParent;
    private GameObject boxPrefab;
    [SerializeField]
    private int maxCount = 10;
    private int currentCount = 0;
    private float maxTime = 0;
    private int preHitCount;
    // Use this for initialization
    void Start()
    {

        boxPrefab = Resources.Load<GameObject>("RolePerfabs/Items/box");
        Debug.Log(boxParent);
        preHitCount = PlayerPrefs.GetInt("PreCount", 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentCount < maxCount)
        {
            if (Time.time > maxTime)
            {
                GameObject go = Instantiate<GameObject>(boxPrefab);
                go.transform.SetParent(boxParent);
                go.transform.position = boxCreateArea.TransformPoint(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                BoxCtrl boxCtrl = go.GetComponent<BoxCtrl>();
                boxCtrl.Onhit = HitBox;
                maxTime += 0.5f;
                currentCount++;

            }

        }
    }

    private void HitBox(GameObject go)
    {
        Destroy(go);
        preHitCount++;
        PlayerPrefs.SetInt("PreCount", preHitCount);
        Debug.Log("一共抓了：" + preHitCount);
        currentCount--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHeadBarInfoView : MonoBehaviour
{

    public Image HeadImage;

    public Slider HPSlider;

    public Slider MPSlider;

    public Text GoldText;

    public Text MoneyText;

    public Text NameText;

    public Text LevelText;

    public static UIPlayerHeadBarInfoView Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUI(string name, int jobId, int level, int gold, int money, int curHp, int maxHp, int curMp, int maxMP)
    {
        NameText.text = name;
        GoldText.text = gold.ToString();
        MoneyText.text = money.ToString();
        HPSlider.value = curHp / (float)maxHp;
        MPSlider.value = curMp / (float)maxMP;
        LevelText.text = string.Format("Lv:{0}", level.ToString());
    }

    public void SetHP(int curHp, int maxHp)
    {
        HPSlider.value = curHp / (float)maxHp;
    }
    public void SetMP(int curMp, int maxMP)
    {
        MPSlider.value = curMp / (float)maxMP;
    }
    private void OnDestroy()
    {
        HeadImage = null;
        HPSlider = null;
        MPSlider = null;
        MoneyText = null;
        GoldText = null;
        NameText = null;
        LevelText = null;
    }
}

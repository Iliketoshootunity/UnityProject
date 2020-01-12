using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameLevelGradeEntity
{

    public GameLevelGrade EnumGrade { get { return (GameLevelGrade)Grade; } }

    #region 奖励的装备 材料 道具
    private List<GoodsEntity> m_RewardEquipList;

    public List<GoodsEntity> RewardEquipList
    {
        get
        {
            if (m_RewardEquipList == null)
            {
                m_RewardEquipList = new List<GoodsEntity>();
                string[] arr = Equip.Split('|');
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] equipArr = arr[i].Split('_');
                        if (equipArr.Length < 3) continue;
                        GoodsEntity entity = new GoodsEntity();
                        entity.GoodsId = equipArr[0].ToInt();
                        entity.GoodsName = EquipDBModel.Instance.Get(entity.GoodsId).Name;
                        entity.GoodsProbability = equipArr[1].ToInt();
                        entity.GoodsCount = equipArr[2].ToInt();
                        m_RewardEquipList.Add(entity);
                    }

                }
            }
            return m_RewardEquipList;
        }
    }

    private List<GoodsEntity> m_RewardItemList;

    public List<GoodsEntity> RewardItemList
    {
        get
        {
            if (m_RewardItemList == null)
            {
                m_RewardItemList = new List<GoodsEntity>();
                string[] arr = Item.Split('|');
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] equipArr = arr[i].Split('_');
                        if (equipArr.Length < 3) continue;
                        GoodsEntity entity = new GoodsEntity();
                        entity.GoodsId = equipArr[0].ToInt();
                        entity.GoodsName = ItemDBModel.Instance.Get(entity.GoodsId).Name;
                        entity.GoodsProbability = equipArr[1].ToInt();
                        entity.GoodsCount = equipArr[2].ToInt();
                        m_RewardItemList.Add(entity);
                    }

                }
            }
            return m_RewardItemList;
        }
    }


    private List<GoodsEntity> m_RewardMateriaList;

    public List<GoodsEntity> RewardMateriaList
    {
        get
        {
            if (m_RewardMateriaList == null)
            {
                m_RewardMateriaList = new List<GoodsEntity>();
                string[] arr = Material.Split('|');
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] equipArr = arr[i].Split('_');
                        if (equipArr.Length < 3) continue;
                        GoodsEntity entity = new GoodsEntity();
                        entity.GoodsId = equipArr[0].ToInt();
                        entity.GoodsName = MaterialDBModel.Instance.Get(entity.GoodsId).Name;
                        entity.GoodsProbability = equipArr[1].ToInt();
                        entity.GoodsCount = equipArr[2].ToInt();
                        m_RewardMateriaList.Add(entity);
                    }

                }
            }
            return m_RewardMateriaList;
        }
    }

    #endregion
}

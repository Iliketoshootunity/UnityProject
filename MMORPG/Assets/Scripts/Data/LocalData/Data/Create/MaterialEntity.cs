
//===================================================
//作    者：肖海林 
//创建时间：2018-11-06 10:56:28
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;
/// <summary>
/// biao01实体
/// </summary>
 public partial class MaterialEntity : AbstractEntity
 {
	/// <summary>
	/// 名称
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	/// 材料品阶类型
	/// </summary>
	public int Quality { get; set; }
	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }
	/// <summary>
	/// 在商店中的图标
	/// </summary>
	public string Image { get; set; }
	/// <summary>
	/// 材料类别
	/// </summary>
	public int Type { get; set; }
	/// <summary>
	/// 材料固定能力的种类
	/// </summary>
	public int FixedType { get; set; }
	/// <summary>
	/// 材料固定能力增加的数值
	/// </summary>
	public int FixedAddValue { get; set; }
	/// <summary>
	/// 最大堆叠数量
	/// </summary>
	public int maxAmount { get; set; }
	/// <summary>
	/// 背包陈列顺序
	/// </summary>
	public int packSort { get; set; }
	/// <summary>
	/// 合成后的材料ID_个数
	/// </summary>
	public string CompositionProps { get; set; }
	/// <summary>
	/// 合成该物时所需的材料ID
	/// </summary>
	public int CompositionMaterialID { get; set; }
	/// <summary>
	/// 合成后需要消耗的金币系数
	/// </summary>
	public string CompositionGold { get; set; }
	/// <summary>
	/// 售价
	/// </summary>
	public int SellMoney { get; set; }
  }

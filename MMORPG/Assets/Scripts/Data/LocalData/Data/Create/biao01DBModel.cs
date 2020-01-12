
 //===================================================
//作    者：肖海林
//创建时间：2018-10-17 15:36:33
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// biao01数据管理
/// </summary>
public partial class biao01DBModel : AbstractDBModel<biao01DBModel, biao01Entity>
 {
	/// <summary>
	/// 文件名称
	/// </summary>
	protected override string FileName { get { return "biao01.data"; } }
	/// <summary>
	/// 创建实体
	/// </summary>
	/// <param name="parse"></param>
	/// <returns></returns>
	protected override biao01Entity MakeEntity(GameDataTableParser parse)
	{
		biao01Entity entity = new biao01Entity();
		entity.Id = parse.GetFieldValue("Id").ToInt();
		entity.Name = parse.GetFieldValue("Name");
		entity.Price = parse.GetFieldValue("Price").ToFloat();
		entity.PciName = parse.GetFieldValue("PciName");
		entity.Des = parse.GetFieldValue("Des");
		return entity;
 	}
}

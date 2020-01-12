using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDBModel : AbstractDBModel<ProductDBModel, ProductEntity>
{


    protected override string FileName
    {
        get
        {
            return "biao01.data";
        }
    }

    protected override ProductEntity MakeEntity(GameDataTableParser parser)
    {
        ProductEntity entity = new ProductEntity();
        entity.Id = parser.GetFieldValue("Id").ToInt();
        entity.Name = parser.GetFieldValue("Name");
        entity.Price = parser.GetFieldValue("Price").ToLong();
        entity.PciName = parser.GetFieldValue("PciName");
        entity.Des = parser.GetFieldValue("Des");
        return entity;
    }
}

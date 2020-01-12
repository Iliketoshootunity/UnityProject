using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoMonster : RoleInfoBase
{

    public SpriteEntity SpriteEntity;

    public RoleInfoMonster(SpriteEntity spriteEntity) : base()
    {
        SpriteEntity = spriteEntity;
    }

}

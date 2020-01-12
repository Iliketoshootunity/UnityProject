using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoleAI  {

    RoleCtrl CurRoleCtrl { get; set; }

    void DoAI();
}

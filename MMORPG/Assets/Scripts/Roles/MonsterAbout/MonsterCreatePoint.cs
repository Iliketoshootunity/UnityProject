using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreatePoint : MonoBehaviour
{
    [SerializeField]
    private int maxCounrt = 3;
    private int currenCount = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (currenCount < maxCounrt)
        {
            GameObject go = RoleMgr.Instance.LoadRole(RoleType.Monster, "Role_Monster_01");
            go.transform.SetParent(this.transform);
            go.transform.position = transform.TransformPoint(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(-0.5f, 0.5f));
            RoleCtrl role = go.GetComponent<RoleCtrl>();
            role.BornPoint = go.transform.position;
            //RoleInfoBase roleInfo = new RoleInfoMonster() { RoleServerID = DateTime.Now.Ticks, RoleID = 1, NickName = "Õ∂ Èµ¡ΩŸ", CurHP = 100, MaxHP = 100 };
            //IRoleAI ai = new RoleMonsterAI(role);
            //role.Init(RoleType.Monster, roleInfo, ai);
            //currenCount++;
            //role.OnRoleDie += OnRoleDie;
        }


    }

    private void OnRoleDie(RoleCtrl obj)
    {
        Destroy(obj.gameObject);
        currenCount--;

    }
}

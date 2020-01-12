using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test01 : MonoBehaviour {

    public Transform Player;
    public Transform Enemy;
    public Transform Prefab;
	// Use this for initialization
	void Start () {

        for (int i = 0; i < 20; i++)
        {
            Vector3 pos = GameUtil.GetTargetSectorPoint(Enemy.position, Player.position, 3);
            GameObject go= Instantiate<GameObject>(Prefab.gameObject);
            go.transform.position = pos;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

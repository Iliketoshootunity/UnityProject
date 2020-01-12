using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Camera : MonoBehaviour {


    public static UI_Camera Instance;
    [HideInInspector]
    public Camera Camera;
    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        Camera = GetComponent<Camera>();


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

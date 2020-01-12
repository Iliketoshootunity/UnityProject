using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISmallMapView : MonoBehaviour {

    public static UISmallMapView Instance;
    private void Awake()
    {
        Instance = this;
    }

}

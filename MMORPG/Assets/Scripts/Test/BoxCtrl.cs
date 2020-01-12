using UnityEngine;
using System.Collections;

public class BoxCtrl : MonoBehaviour
{

    public System.Action<GameObject> Onhit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  

    }

    public void OnHit()
    {
        if(Onhit!=null)
        {
            Onhit(this.gameObject);
        }
    }
}

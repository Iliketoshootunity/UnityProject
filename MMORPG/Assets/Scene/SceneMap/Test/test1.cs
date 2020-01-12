using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{

    private Animator animator;
    private bool isInput;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (isInput)
        {
            if (state.IsName("001"))
            {
                Debug.Log("001");
                animator.SetTrigger("001");
            }
            if (state.IsName("002"))
            {
                Debug.Log("002");
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("001");
            isInput = true;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hitarry = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Role"));

        if (hitarry.Length > 0)
        {
            Debug.Log("Role");
        }


    }
}

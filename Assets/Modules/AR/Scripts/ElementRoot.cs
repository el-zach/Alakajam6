using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementRoot : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetState(bool togOn)
    {
        if(animator)
            animator.SetBool("Show", togOn);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAnimation : MonoBehaviour
{    
    Animator animator;
    BeatManager bManager;
    void Start()
    {
        animator = GetComponent<Animator>();
        bManager = BeatManager.Instance;
    }
    
    void Update()
    {
        if (bManager.BeatGracePeriod)
        {
            animator.SetBool("Fade", false);
        }
        else
        {
            animator.SetBool("Fade", true);
        }
    }
}

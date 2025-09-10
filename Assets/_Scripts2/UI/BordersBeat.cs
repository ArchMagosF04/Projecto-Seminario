using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BordersBeat : MonoBehaviour
{
    private bool OnBeat = true;
    private Animator animator;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
       image = GetComponent<Image>();
        animator = GetComponent<Animator>();
        BeatManager.Instance.intervals[2].OnBeatEvent += BeatEffectOn;
        BeatManager.Instance.OnCorrectBeat += GoodHit;
        BeatManager.Instance.OnWrongBeat += BadHit;
    }

    // Update is called once per frame
    void Update()
    {
        if (OnBeat)
        {
            OnBeat = false;
            animator.SetTrigger("Off");
            
        }
    }

    private void BeatEffectOn()
    {
        image.color = Color.white;
        //gameObject.SetActive(true);
        animator.SetTrigger("On");
        OnBeat = true;
    }

    private void GoodHit()
    {
        //image.color = Color.green;
        animator.SetTrigger("Green");
    }

    private void BadHit()
    {
        animator.SetTrigger("Red");
    }
}

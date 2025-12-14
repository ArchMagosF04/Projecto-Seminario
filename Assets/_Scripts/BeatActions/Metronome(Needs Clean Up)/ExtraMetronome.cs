using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMetronome : MonoBehaviour
{
    //[SerializeField] MetronomeVertical centralMetronome;

    [SerializeField, Range(0f, 0.5f)] private float lingeringBeatGraceTime = 0.2f;
    private float extraGraceTime;

    private Animator anim;
    [SerializeField]private SpriteRenderer[] markerSprites;

    private float startOfBeat;
    private float beatDuration;

    [SerializeField] private int currentInterval = 0;

    //[SerializeField] private bool finalMetronome;
    [SerializeField] private bool animate;
    private bool busy = false;

    

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //Time.timeScale = 0.25f;
        beatDuration = 60f / BeatManager.Instance.BPM;
        extraGraceTime = beatDuration * lingeringBeatGraceTime;
        //centralMetronome.OnExtraBeat += Isbusy;
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[currentInterval].OnBeatEvent += MetronomeBeat;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[currentInterval].OnBeatEvent -= MetronomeBeat;

    }

    private void MetronomeBeat()
    {      
        if(animate)anim.SetTrigger("Beat");
        //if (finalMetronome) centralMetronome.MoveBar();
    }

    private IEnumerator LingeringGrace(float time)
    {
        yield return new WaitForSeconds(time);
        BeatManager.Instance.ToggleGracePeriod(false);
        markerSprites[0].color = Color.white;
        busy = false;       

    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MetronomeBar")
        {            
            BeatManager.Instance.ToggleGracePeriod(true);
            if (collision != null)
            {
                collision.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

                renderer.color = Color.green;
                
                busy = true;

                
                //if (finalMetronome)
                //{
                //    centralMetronome.ResetMarkers();                    
                //}
                
            }
        }                 

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "MetronomeBar")
        {
            
            if (collision != null)
            {
                busy = true;
                BeatManager.Instance.ToggleGracePeriod(true);                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MetronomeBar")
        {
            if (collision != null)
            {
                collision.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

                renderer.color = Color.white;
                StartCoroutine(LingeringGrace(lingeringBeatGraceTime));
            }
            
        }
       
             
    }

    private bool Isbusy()
    {
        return busy;
    }
}

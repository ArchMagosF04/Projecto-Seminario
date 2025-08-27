using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMetronome : MonoBehaviour
{
    [SerializeField] BeatMetronome centralMetronome;

    [SerializeField, Range(0f, 0.5f)] private float lingeringBeatGraceTime = 0.2f;
    private float extraGraceTime;

    private Animator anim;
    [SerializeField]private SpriteRenderer[] markerSprites;

    private float startOfBeat;
    private float beatDuration;

    [SerializeField] private int currentInterval = 0;

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
        anim.SetTrigger("Beat");      
    }

    private IEnumerator LingeringGrace(float time)
    {
        yield return new WaitForSeconds(time);
        BeatManager.Instance.ToggleGracePeriod(false);
        busy = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

        renderer.color = Color.yellow;

        BeatManager.Instance.ToggleGracePeriod(true);
        busy = true;

        centralMetronome.OnExtraBeat+= Isbusy;

        StartCoroutine(LingeringGrace(lingeringBeatGraceTime));

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

        renderer.color = Color.white;               
    }

    private bool Isbusy()
    {
        return busy;
    }
}

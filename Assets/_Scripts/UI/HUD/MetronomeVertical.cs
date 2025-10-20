using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeVertical : MonoBehaviour
{
    [SerializeField] private Transform markerSpawnPointRight;
    [SerializeField] private GameObject markerRight;

    [SerializeField, Range(0f, 0.5f)] private float lingeringBeatGraceTime = 0.2f;
    private float extraGraceTime;

    private Animator anim;
    private SpriteRenderer markerSprite;

    private float startOfBeat;
    private float beatDuration;

    [SerializeField] private int currentInterval = 0;

    [SerializeField] ExtraMetronome[] extraMetronomes;

    [SerializeField] private bool animate;

    public event Func<bool> OnExtraBeat;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        if (markerRight != null)
        {
            markerSprite = markerRight.GetComponent<SpriteRenderer>();
        }        
    }

    private void Start()
    {
        beatDuration = 120f / BeatManager.Instance.BPM;
        extraGraceTime = beatDuration * lingeringBeatGraceTime;
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[currentInterval].OnBeatEvent += MetronomeBeat;
        BeatManager.Instance.OnWrongBeat += OnBeatMiss;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[currentInterval].OnBeatEvent -= MetronomeBeat;
        BeatManager.Instance.OnWrongBeat -= OnBeatMiss;
    }

    private void MetronomeBeat()
    {
        ResetMarkers();

        if(animate)anim.SetTrigger("Beat");

        if (markerRight != null)
        {
            StartCoroutine(LerpRightToPosition());
        }

    }

    public void ResetMarkers()
    {
        if (markerRight != null)
        {
            markerRight.transform.position = markerSpawnPointRight.position;

        }

        if (markerSprite != null)
        {
            markerSprite.color = Color.white;
        }

        if (markerRight != null)
        {
            StartCoroutine(LerpRightToPosition());
        }

        StartCoroutine(LingeringGrace(extraGraceTime));
    }

    private IEnumerator LingeringGrace(float time)
    {
        yield return new WaitForSeconds(time);
        BeatManager.Instance.ToggleGracePeriod(false);
    }

    private IEnumerator LerpRightToPosition()
    {
        startOfBeat = Time.time;

        while (Time.time - startOfBeat < beatDuration)
        {
            float elapse = Time.time - startOfBeat;

            markerRight.transform.position = Vector2.Lerp(markerSpawnPointRight.position, transform.position, elapse / beatDuration);

            yield return null;
        }
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Enemy")
        {

            if (markerSprite != null)
            {
                markerSprite.color = Color.green;
            }

            BeatManager.Instance.ToggleGracePeriod(true);

        }

    }

    [ContextMenu("OnBeatMiss")]
    public void OnBeatMiss()
    {
        if (OnExtraBeat())
        {
            return;            
        }

        markerSprite.color = Color.red;

        StopAllCoroutines();
    }

    public void MoveBar()
    {
        StartCoroutine(LerpRightToPosition());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMetronome : MonoBehaviour
{
    [SerializeField] private Transform markerSpawnPointRight;
    [SerializeField] private GameObject markerRight;

    [SerializeField] private Transform markerSpawnPointLeft;
    [SerializeField] private GameObject markerLeft;

    [SerializeField, Range(0f, 0.5f)] private float lingeringBeatGraceTime = 0.2f;
    private float extraGraceTime;

    private Animator anim;
    private SpriteRenderer[] markerSprites = new SpriteRenderer[2];

    private float startOfBeat;
    private float beatDuration;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        markerSprites[0] = markerRight.GetComponent<SpriteRenderer>();
        markerSprites[1] = markerLeft.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        beatDuration = 60f / BeatManager.Instance.BPM;
        extraGraceTime = beatDuration * lingeringBeatGraceTime;
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent += MetronomeBeat;
        BeatManager.Instance.OnWrongBeat += OnBeatMiss;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= MetronomeBeat;
        BeatManager.Instance.OnWrongBeat -= OnBeatMiss;
    }

    private void MetronomeBeat()
    {
        ResetMarkers();

        anim.SetTrigger("Beat");

        StartCoroutine(LerpRightToPosition());
        StartCoroutine(LerpLeftToPosition());
    }

    private void ResetMarkers()
    {
        markerRight.transform.position = markerSpawnPointRight.position;

        foreach (SpriteRenderer spriteRenderer in markerSprites)
        {
            spriteRenderer.color = Color.white;
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

    private IEnumerator LerpLeftToPosition()
    {
        startOfBeat = Time.time;

        while (Time.time - startOfBeat < beatDuration)
        {
            float elapse = Time.time - startOfBeat;
            markerLeft.transform.position = Vector2.Lerp(markerSpawnPointLeft.position, transform.position, elapse / beatDuration);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (SpriteRenderer spriteRenderer in markerSprites)
        {
            spriteRenderer.color = Color.green;
            BeatManager.Instance.ToggleGracePeriod(true);
        }
    }

    [ContextMenu("OnBeatMiss")]
    private void OnBeatMiss()
    {
        foreach (SpriteRenderer spriteRenderer in markerSprites)
        {
            spriteRenderer.color = Color.red;
        }

        StopAllCoroutines();
    }
}

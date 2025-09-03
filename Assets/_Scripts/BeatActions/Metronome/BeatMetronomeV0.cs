using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMetronomeV0 : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform markerSpawnPointRight;
    [SerializeField] private Transform markerSpawnPointLeft;

    [Header("Markers")]
    [SerializeField] private GameObject markerRight;
    [SerializeField] private GameObject markerLeft;

    [Header("Variables")]
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
        BeatManager.Instance.intervals[0].OnBeatEvent += MetronomeFullBeat;
        BeatManager.Instance.OnWrongBeat += OnBeatMiss;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= MetronomeFullBeat;
        BeatManager.Instance.OnWrongBeat -= OnBeatMiss;
    }

    private void MetronomeFullBeat()
    {
        ResetMarkers();

        anim.SetTrigger("Beat");

        StartCoroutine(LerpFullRightToPosition());
        StartCoroutine(LerpFullLeftToPosition());
    }

    private void ResetMarkers()
    {
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

    private IEnumerator LerpFullRightToPosition()
    {
        startOfBeat = Time.time;

        while (Time.time - startOfBeat < beatDuration)
        {
            float elapse = Time.time - startOfBeat;
            markerRight.transform.position = Vector2.Lerp(markerSpawnPointRight.position, transform.position, elapse / beatDuration);

            yield return null;
        }
    }

    private IEnumerator LerpFullLeftToPosition()
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
    }
}

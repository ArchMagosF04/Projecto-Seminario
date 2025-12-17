using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SquashAndStretch;

public class BeatBorder : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvas;

    [Header("Settings")]
    [SerializeField] private float animationDuration;
    [SerializeField] private bool durationProportionalToBeatDuration;
    [SerializeField, Range(0, 1f)] private float proportionalValue;


    [Space(5)]

    [SerializeField]
    private AnimationCurve _fadeCurve =
        new AnimationCurve(new Keyframe(0f, 0.5f), new Keyframe(0.1f, 1f), new Keyframe(0.5f, 0.85f), new Keyframe(1f, 0f));

    private Coroutine fadeCoroutine;
    private float proportionalDuration;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvas.alpha = 0f;

        proportionalDuration = (60 / BeatManager.Instance.BPM) * proportionalValue;
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent += PlayFadeAnimation;
    }

    private void OnDisable()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        BeatManager.Instance.intervals[0].OnBeatEvent -= PlayFadeAnimation;
    }

    public void PlayFadeAnimation()
    {
        CheckForAndStartCoroutine();
    }

    private void CheckForAndStartCoroutine()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            canvas.alpha = 0f;
        }

        fadeCoroutine = StartCoroutine(FadeInOutCoroutine());
    }

    private IEnumerator FadeInOutCoroutine()
    {
        float elapsedTime = 0f;

        float duration = durationProportionalToBeatDuration ? proportionalDuration : animationDuration;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float curvePosition;

            curvePosition = elapsedTime / duration;

            float curveValue = _fadeCurve.Evaluate(curvePosition);

            canvas.alpha = Mathf.Clamp01(curveValue);

            yield return null;
        }
    }
}

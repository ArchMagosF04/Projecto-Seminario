using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SquashAndStretch : MonoBehaviour
{
    [Header("Squash and Stretch Core")]
    [SerializeField, Tooltip("Defaults to current GameObject if not set")] private Transform transformToAffect;
    [SerializeField] private SquashStretchAxis axisToAffect = SquashStretchAxis.Y;
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.25f;
    [SerializeField] private bool canBeOverwritten;
    [SerializeField] private bool playOnStart;
    [SerializeField] private bool usesUnscaledDeltaTime;

    [Tooltip("If off the effect rolls a random number to see if it will be performed.")]
    [SerializeField] private bool playsEveryTime = true;
    [SerializeField, Range(0, 100f)] private float chanceToPlay = 100f;

    [Tooltip("Controls whether it will listen to the static event that activates all objects with this script.")]
    [SerializeField] private bool ignoreGlobalPlayEvent = true;

    [Flags]
    public enum SquashStretchAxis
    {
        None = 0,
        X = 1, 
        Y = 2,
        Z = 4
    }

    [Header("Animation Settings")]
    [SerializeField] private float initialScale = 1f;
    [SerializeField] private float maximumScale = 1.3f;
    [SerializeField] private bool resetToInitialScaleAfterAnimation = true;
    [SerializeField] private bool reverseAnimationCurveAfterPlaying;
    private bool isReversed;

    [SerializeField] private AnimationCurve squashAndStretchCurve = 
        new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 1f) , new Keyframe(1f, 0f));

    [Header("Looping Settings")]
    [SerializeField] private bool shouldLoop;
    [SerializeField] private float loopingDelay = 0.5f;

    private Coroutine squashStretchCoroutine;
    private WaitForSeconds loopingDelayWaitForSeconds;
    private Vector3 initialScaleVector;

    private bool affectX => (axisToAffect & SquashStretchAxis.X) != 0;
    private bool affectY => (axisToAffect & SquashStretchAxis.Y) != 0;
    private bool affectZ => (axisToAffect & SquashStretchAxis.Z) != 0;


    private static event Action _squashAndStretchAllObjectsLikeThis;

    [Header("Events"), Space(10)]
    public UnityEvent OnAnimationFinished;

    private void Awake()
    {
        if (transformToAffect == null) transformToAffect = transform;

        initialScaleVector = transformToAffect.localScale;
        loopingDelayWaitForSeconds = new WaitForSeconds(loopingDelay);
    }

    public static void SquashAndStretchAllObjcetsLikeThis()
    {
        _squashAndStretchAllObjectsLikeThis?.Invoke();
    }

    private void OnEnable()
    {
        if (ignoreGlobalPlayEvent) return;

        _squashAndStretchAllObjectsLikeThis += PlaySquashAndStretch;
    }

    private void OnDisable()
    {
        if (squashStretchCoroutine != null) StopCoroutine(squashStretchCoroutine);

        if (ignoreGlobalPlayEvent) return;

        _squashAndStretchAllObjectsLikeThis -= PlaySquashAndStretch;
    }

    private void Start()
    {
        if (playOnStart) CheckForAndStartCoroutine();
    }

    [ContextMenu("Play Squash and Stretch")]
    public void PlaySquashAndStretch()
    {
        if (shouldLoop && !canBeOverwritten) return;

        CheckForAndStartCoroutine();
    }

    private void CheckForAndStartCoroutine()
    {
        if (axisToAffect == SquashStretchAxis.None)
        {
            Debug.Log("Axis to affect is set to None.", gameObject);
            return;
        }

        if (squashStretchCoroutine != null)
        {
            StopCoroutine(squashStretchCoroutine);
            if (playsEveryTime && resetToInitialScaleAfterAnimation)
            {
                transform.localScale = initialScaleVector;
            }
        }

        squashStretchCoroutine = StartCoroutine(SquashAndStretchEffect());
    }

    private IEnumerator SquashAndStretchEffect()
    {
        do
        {
            if (!playsEveryTime)
            {
                float random = UnityEngine.Random.Range(0, 100f);
                if (random > chanceToPlay)
                {
                    yield return null;
                    continue;
                }
            }

            if (reverseAnimationCurveAfterPlaying)
            {
                isReversed = !isReversed;
            }

            float elapsedTime = 0f;
            Vector3 originalScale = initialScaleVector;
            Vector3 modifiedScale = originalScale;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += usesUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;

                float curvePosition;

                if (isReversed) curvePosition = 1 - (elapsedTime / animationDuration);
                else curvePosition = elapsedTime / animationDuration;

                    float curveValue = squashAndStretchCurve.Evaluate(curvePosition);
                float remappedValue = initialScale + (curveValue * (maximumScale - initialScale));

                float minimumThresehold = 0.0001f;
                if (Mathf.Abs(remappedValue) < minimumThresehold) remappedValue = minimumThresehold;

                if (affectX) modifiedScale.x = originalScale.x * remappedValue;
                else modifiedScale.x = originalScale.x / remappedValue;

                if (affectY) modifiedScale.y = originalScale.y * remappedValue;
                else modifiedScale.y = originalScale.y / remappedValue;

                if (affectZ) modifiedScale.z = originalScale.z * remappedValue;
                else modifiedScale.z = originalScale.z / remappedValue;

                transformToAffect.localScale = modifiedScale;

                yield return null;
            }

            if (resetToInitialScaleAfterAnimation) transformToAffect.localScale = originalScale;

            OnAnimationFinished?.Invoke();

            if (shouldLoop) yield return loopingDelayWaitForSeconds;

        } while (shouldLoop);
    }

    public void SetLooping(bool loop) => shouldLoop = loop;
}

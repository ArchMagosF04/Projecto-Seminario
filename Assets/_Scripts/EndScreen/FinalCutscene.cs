using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutscene : MonoBehaviour
{
    [Header("Cutscene Slides")]
    [SerializeField] private SlideSceneSettings[] cutSceneSlides;

    [Header("Cutscene Settings")]
    [SerializeField] private float startDelay = 5f;
    [SerializeField] private float finalDelay = 2.5f;

    private float timeSinceSlideAppeared;
    private int currentCanvasSlide;

    private bool slideInFullDisplay;

    private bool cutsceneEnded;

    private SoundSource endMusicSource;

    private void Awake()
    {
        endMusicSource = GetComponent<SoundSource>();

        cutsceneEnded = false;
        currentCanvasSlide = 0;
        slideInFullDisplay = false;

        for (int i = 0; i < cutSceneSlides.Length; i++)
        {
            cutSceneSlides[i].sceneSlide.alpha = 0;
        }
    }

    private void Start()
    {
        endMusicSource.Play();

        StartCoroutine(FadeCanvasSlide(startDelay, cutSceneSlides[0].sceneSlide, 0, 1, cutSceneSlides[0].fadeInDuration));
    }

    private void Update()
    {
        if (GameInputManager.Instance.JumpInput)
        {
            GameInputManager.Instance.UseJumpInput();
            timeSinceSlideAppeared = -100;
        }

        if (Time.time > timeSinceSlideAppeared + cutSceneSlides[currentCanvasSlide].slideDuration && slideInFullDisplay && !cutsceneEnded)
        {
            if (currentCanvasSlide < cutSceneSlides.Length - 1)
            {
                currentCanvasSlide++;
                slideInFullDisplay = false;
                StartCoroutine(FadeCanvasSlide(0, cutSceneSlides[currentCanvasSlide].sceneSlide, 0, 1, cutSceneSlides[currentCanvasSlide].fadeInDuration));
            }
            else
            {
                cutsceneEnded = true;
                endMusicSource.Stop();
                StartCoroutine(FadeCanvasSlide(cutSceneSlides[currentCanvasSlide].fadeInDuration / 2, cutSceneSlides[currentCanvasSlide].sceneSlide, 1, 0, cutSceneSlides[currentCanvasSlide].fadeInDuration));
            }
        }
    }

    private IEnumerator FadeCanvasSlide(float initialDelay, CanvasGroup canvas, float start, float end, float duration)
    {
        yield return new WaitForSeconds(initialDelay);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(start, end, elapsedTime / duration);

            yield return null;
        }

        canvas.alpha = end;
        slideInFullDisplay = true;
        timeSinceSlideAppeared = Time.time;

        if (cutsceneEnded)
        {
            yield return new WaitForSeconds(finalDelay);
            AsyncSceneLoader.Instance.LoadLevel(0);
        }
    }
}

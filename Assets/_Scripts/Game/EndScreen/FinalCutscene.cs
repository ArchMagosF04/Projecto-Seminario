using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutscene : MonoBehaviour
{
    [Header("Cutscene Slides")]
    [SerializeField] private SlideSceneSettings[] cutSceneSlides;

    private float timeSinceSlideAppeared;
    private int currentCanvasSlide;

    private bool slideInFullDisplay;

    private bool cutsceneEnded;

    private void Awake()
    {
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
        StartCoroutine(FadeCanvasSlide(cutSceneSlides[0].sceneSlide, 0, 1, cutSceneSlides[0].fadeInDuration));
    }

    private void Update()
    {
        if (InputManager.Instance.JumpInput)
        {
            InputManager.Instance.UseJumpInput();
            timeSinceSlideAppeared = -100;
        }

        if (Time.time > timeSinceSlideAppeared + cutSceneSlides[currentCanvasSlide].slideDuration && slideInFullDisplay && !cutsceneEnded)
        {
            if (currentCanvasSlide < cutSceneSlides.Length - 1)
            {
                currentCanvasSlide++;
                slideInFullDisplay = false;
                StartCoroutine(FadeCanvasSlide(cutSceneSlides[currentCanvasSlide].sceneSlide, 0, 1, cutSceneSlides[currentCanvasSlide].fadeInDuration));
            }
            else
            {
                cutsceneEnded = true;
                StartCoroutine(FadeCanvasSlide(cutSceneSlides[currentCanvasSlide].sceneSlide, 1, 0, cutSceneSlides[currentCanvasSlide].fadeInDuration));
            }
        }
    }

    private IEnumerator FadeCanvasSlide(CanvasGroup canvas, float start, float end, float duration)
    {
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

        if (cutsceneEnded) SceneLoaderManager.Instance.LoadSceneByIndex(0);
    }
}

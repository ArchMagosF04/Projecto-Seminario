using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [Header("Flash Settings")]
    [ColorUsage(true, true)]
    [SerializeField] private Color[] flashColor;
    [SerializeField, Range(0f, 1f)] private float flashTime = 0.25f;
    [SerializeField] private AnimationCurve flashSpeedCurve;

    [Header("Afected Sprites")]
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    private Material[] materials;

    private Coroutine spriteFlashCoroutine;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        materials = new Material[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }

    public void CallSpriteFlash(int index)
    {
        if (spriteFlashCoroutine != null)
        {
            StopCoroutine(spriteFlashCoroutine);
        }
        spriteFlashCoroutine = StartCoroutine(SpriteFlasher(index));
    }

    private IEnumerator SpriteFlasher(int index)
    {
        SetFlashColor(index);

        float currentFlashAmount = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = flashSpeedCurve.Evaluate(elapsedTime / flashTime);
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    private void SetFlashColor(int colorIndex)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_FlashColor", flashColor[colorIndex]);
        }
    }

    private void SetFlashAmount(float amount)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}

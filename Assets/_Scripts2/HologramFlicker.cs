using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramFlicker : MonoBehaviour
{
    SpriteRenderer sprite;

    [SerializeField] private float fadeModifier = 0.33f;
    private float maxFade = 0.20f;

    [SerializeField] private float fadeDuration = 2;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //BeatManager.Instance.OnCorrectBeat += PaintGreen;
        //BeatManager.Instance.OnWrongBeat += PaintRed;
        //BeatManager.Instance.intervals[0].OnBeatEvent += Solidify;
        StartCoroutine("Solidify");
        //StartCoroutine("Fade");
    }

    IEnumerator Solidify()
    {
        float duration = fadeDuration;
        yield return new WaitUntil(() => BeatManager.Instance.BeatGracePeriod);
        //StopCoroutine("Fade");
        //sprite.color = Color.white;
        //Debug.Log("ShowBorders");        
        while (duration > 0)
        {
            if (!BeatManager.Instance.BeatGracePeriod) break;
            yield return new WaitForSeconds(0.015f);
            Color color = sprite.color;
            if (color.a + fadeModifier < 1)
            {
                color.a = Mathf.Clamp01(color.a += fadeModifier);
            }
            else
            {
                color.a = 1;
            }
            sprite.color = color;

            //color.a = Mathf.Clamp01(1);
            sprite.color = color;            
        }        
        StartCoroutine(Fade());
        yield return null;
    }

    IEnumerator Fade()
    {
        float duration = fadeDuration;
        yield return new WaitWhile(() => !BeatManager.Instance.BeatGracePeriod);
        StopCoroutine("Solidify");        
        while (duration > 0)
        {
            if (BeatManager.Instance.BeatGracePeriod) break;
            yield return new WaitForSeconds(0.015f);
            Color color = sprite.color;
            if (color.a - fadeModifier > maxFade)
            {
                color.a = Mathf.Clamp01(color.a -= fadeModifier);
            }
            else
            {
                color.a = maxFade;
            }
            sprite.color = color;
            //Debug.Log("FadeBorders");
            duration -= 0.025f;            
        }
        StartCoroutine(Solidify());
        yield return null;
    }
}

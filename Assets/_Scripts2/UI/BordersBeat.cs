using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BordersBeat : MonoBehaviour
{
    [SerializeField] Image borders;

    [SerializeField] private float fadeModifier = 0.33f;

    [SerializeField] private float fadeDuration= 3;

    private void Awake()
    {
        
    }

    private void Start()
    {
        BeatManager.Instance.OnCorrectBeat += PaintGreen;
        BeatManager.Instance.OnWrongBeat += PaintRed;
        BeatManager.Instance.intervals[0].OnBeatEvent += ShowBorders;
    }

    private void PaintGreen()
    {
        borders.color = Color.green;
    }

    private void PaintRed()
    {
        borders.color = Color.red;
    }

    private void ShowBorders()
    {        
        StopAllCoroutines();
        borders.color = Color.white;
        //Debug.Log("ShowBorders");
        Color color = borders.color;
        color.a = Mathf.Clamp01(1);
        borders.color = color;
        StartCoroutine(FadeBorders());
    }

    IEnumerator FadeBorders()
    {        
        float duration = fadeDuration;
        while (duration > 0)
        {
            yield return new WaitForSeconds(0.025f);            
            Color color = borders.color;
            color.a = Mathf.Clamp01(color.a -= fadeModifier);
            borders.color = color;
            //Debug.Log("FadeBorders");
            duration-= 0.025f;
        }
    }
}

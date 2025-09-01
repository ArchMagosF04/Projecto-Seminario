using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffToRythm : MonoBehaviour
{
    [SerializeField] BeatDetector beatDetector;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        beatDetector.OnBeat += BeatEffect;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeatEffect(bool status)
    {
        spriteRenderer.enabled = status;
    }
}

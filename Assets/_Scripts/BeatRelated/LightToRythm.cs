using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class LightToRythm : MonoBehaviour
{
    private Light2D light;
    [SerializeField] float intensity;
    private float initialIntensity;
    [SerializeField] BeatDetector detector;

    private void Awake()
    {    
        light = GetComponent<Light2D>();
        initialIntensity = light.intensity;
    }

    // Start is called before the first frame update
    void Start()
    {
        detector.OnBeat += BeatEffect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeatEffect(bool status)
    {
        if (status == true)
        {
            light.intensity = intensity;
        }
        else if (status == false)
        {
            light.intensity = initialIntensity;
        }
    }
}

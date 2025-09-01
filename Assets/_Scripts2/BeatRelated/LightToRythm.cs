using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class LightToRythm : MonoBehaviour
{
    private Light2D _light;
    [SerializeField] float intensity;
    private float initialIntensity;
    [SerializeField] BeatDetector detector;

    private void Awake()
    {    
        _light = GetComponent<Light2D>();
        initialIntensity = _light.intensity;
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
            _light.intensity = intensity;
        }
        else if (status == false)
        {
            _light.intensity = initialIntensity;
        }
    }
}

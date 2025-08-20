using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundData
{
    public string Id;
    public AudioClip Clip;
    public AudioMixerGroup MixerGroup;
    [Range(0f, 1f)] public float Volume = 1f;
    public bool RandomPitch;
    [Range(0f, 0.5f)] public float PitchVariation = 0.1f;
    public bool Loop;
    public bool PlayOnAwake;
    public bool FrequentSound;
}

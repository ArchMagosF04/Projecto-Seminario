using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBuilder
{
    readonly SoundManager soundManager;
    private SoundData soundData;
    private Vector3 position = Vector3.zero;

    public SoundBuilder(SoundManager soundManager)
    {
        this.soundManager = soundManager;
    }

    public SoundBuilder WithSoundData(SoundData soundData)
    {
        this.soundData = soundData;
        return this;
    }

    public SoundBuilder WithPosition(Vector3 position)
    {
        this.position = position; 
        return this;
    }

    public void Play()
    {
        if (!soundManager.CanPlaySound(soundData)) return;

        SoundEmmiter soundEmmiter = soundManager.Get();
        soundEmmiter.Initialize(soundData);
        soundEmmiter.transform.position = position;
        soundEmmiter.transform.parent = SoundManager.Instance.transform;

        if (soundData.RandomPitch) soundEmmiter.WithRandomPitch(-soundData.PitchVariation, soundData.PitchVariation);

        if (soundData.FrequentSound) soundManager.FrequentSoundEmmiters.Enqueue(soundEmmiter);

        soundEmmiter.Play();        
    }
}

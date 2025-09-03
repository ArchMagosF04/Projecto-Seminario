using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    public Action OnCorrectBeat;
    public Action OnWrongBeat;

    [field: SerializeField] public float BPM { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    [field: SerializeField] public bool BeatGracePeriod { get; private set; }

    [field: SerializeField] public Interval[] intervals { get; private set; }

    public float BeatSpeedMultiplier { get; private set; }

    private float normalMusicVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        CalculateAnimationSpeedMultiplier();
        
        normalMusicVolume = AudioSource.volume;
    }

    public void ToggleMusic(bool input)
    {
        if (input) AudioSource.Play();
        else AudioSource.Pause();
    }

    public void IncreaseMusicVolume(float volume)
    {
        AudioSource.volume = volume;
    }

    public void DecreaseMusicVolume(float volume)
    {
        AudioSource.volume = volume;
    }

    public void NormalizeMusicVolume()
    {

    }

    private void Update()
    {
        if (!AudioSource.isPlaying) return;

        foreach (Interval interval in intervals)
        {
            float sampledTime = (AudioSource.timeSamples / (AudioSource.clip.frequency * interval.GetIntervalLength(BPM)));
            interval.CheckForNewInterval(sampledTime);
        }
    }

    [ContextMenu("Test Player Rhythim")]
    public void OnPlayerRhythmicAction()
    {
        if (BeatGracePeriod) OnCorrectBeat?.Invoke();
        else OnWrongBeat?.Invoke();
    }

    public void ToggleGracePeriod(bool value)
    {
        BeatGracePeriod = value;
    }

    protected void CalculateAnimationSpeedMultiplier()
    {
        float secondsPerBeat = 60f / BPM;

        float difference = secondsPerBeat / 0.5f;

        BeatSpeedMultiplier = 1 / difference;
    }
}

using Ami.BroAudio;
using Ami.BroAudio.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    public Action OnCorrectBeat;
    public Action OnWrongBeat;

    [field: Header("Music Settings")]
    [field: SerializeField] public float BPM { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }
    [field: SerializeField] private SoundID musicID;

    [field: Header("Beat Checks")]
    [field: SerializeField] public bool BeatGracePeriod { get; private set; }
    [field: SerializeField] public float sampledTime { get; private set; }

    [field: SerializeField] public Interval[] intervals { get; private set; }

    [SerializeField] private SoundID testBeatSound;

    public float BeatSpeedMultiplier { get; private set; }

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

        BeatSpeedMultiplier = CalculateAnimationSpeedMultiplier();
        BroAudio.Play(musicID);
    }

    [ContextMenu("Find the Music Source")]
    private void FindTheAudioSource()
    {
        AudioPlayer[] audioPlayers = FindObjectsByType<AudioPlayer>(FindObjectsSortMode.None);

        foreach (AudioPlayer audioPlayer in audioPlayers)
        {
            if (audioPlayer.ID == musicID)
            {
                AudioSource = audioPlayer.GetCurrentAudioSource();
            }
        }
    }

    public void PlaySound()
    {
        BroAudio.Play(testBeatSound);
    }

    public void DebugTest(string message)
    {
        Debug.Log(message);
    }

    public void ToggleMusic(bool input)
    {
        if (input) AudioSource.Play();
        else AudioSource.Pause();
    }

    private void Update()
    {
        if (AudioSource == null) FindTheAudioSource();
        else if (AudioSource.isPlaying)
        {
            foreach (Interval interval in intervals)
            {
                sampledTime = (AudioSource.timeSamples / (AudioSource.clip.frequency * interval.GetIntervalLength(BPM)));
                interval.CheckForNewInterval(sampledTime);
            }
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

    public float CalculateAnimationSpeedMultiplier()
    {
        float secondsPerBeat = 60f / BPM;

        float difference = secondsPerBeat / 0.5f;

        return 1 / difference;
    }
}

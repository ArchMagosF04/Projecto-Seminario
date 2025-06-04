using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    [SerializeField] private float bpm;
    [SerializeField] private AudioSource audioSource;
    private Interval[] intervals = new Interval[4];

    [field: SerializeField] public Interval OneBeat { get; private set; }
    [field: SerializeField] public Interval TwoBeat { get; private set; }
    [field: SerializeField] public Interval FourBeat { get; private set; }
    [field: SerializeField] public Interval HalfBeat { get; private set; }


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
    }

    private void Start()
    {
        intervals[0] = OneBeat;
        intervals[1] = TwoBeat;
        intervals[2] = FourBeat;
        intervals[3] = HalfBeat;
    }

    private void Update()
    {
        foreach (Interval interval in intervals)
        {
            float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * interval.GetIntervalLength(bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }
}


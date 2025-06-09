using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    [field: SerializeField] public float BPM { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }
    private Interval[] intervals = new Interval[4];

    private float SampledTime;

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
        //SampledTime = (AudioSource.timeSamples / (AudioSource.clip.frequency * OneBeat.GetIntervalLength(BPM)));
        //OneBeat.CheckForNewInterval(SampledTime);

        foreach (Interval interval in intervals)
        {
            float sampledTime = (AudioSource.timeSamples / (AudioSource.clip.frequency * interval.GetIntervalLength(BPM)));
            interval.CheckForNewInterval(sampledTime);
        }
    }
}

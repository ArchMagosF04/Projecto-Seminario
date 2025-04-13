using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class BeatManager : MonoBehaviour
{
    //private static BeatManager instance;

    //--------------SINGLETON --------------------------------------------------------------------------------------------
    public static BeatManager Instance;

//------------------------------------------------------------------------------------------------------------------------

    public TrackInfo trackInfo;

    private AudioSource track;

    public event Action<float> OnBeat;
    
    private int lastInterval;

    [SerializeField] Intervals[] intervals;

    private void Awake()
    {
        Instance = this;
        track = GameObject.Find("BGM").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var interval in intervals) // Time elapsed in beats
        {
            float sampledTime = track.timeSamples / (track.clip.frequency * interval.GetIntervalLenght(trackInfo.Bpm));
            interval.CheckForNewInterval(sampledTime);
        }
    }

    public void ChangeTrack(TrackInfo newTrack)
    {
        track.Stop();
        track.clip = newTrack.MusicTrack;
        trackInfo = newTrack;
        track.Play();
        
    }

    [System.Serializable]
    public class Intervals
    {
        [SerializeField] private float steps;
        [SerializeField] private UnityEvent trigger;
        private int lastInterval;


        public float GetIntervalLenght(float bpm) // The lenght of the current beat
        {
            float result = 60 / (bpm * steps);
            return result;
        }

        public void CheckForNewInterval(float interval)
        {
            if (Mathf.FloorToInt(interval) != lastInterval)
            {
                lastInterval = Mathf.FloorToInt(interval);
                trigger.Invoke();
                //OnBeat.Invoke();
            }
        }
    }
    
}

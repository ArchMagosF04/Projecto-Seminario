using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class BeatManager : MonoBehaviour
{
    private static BeatManager instance;
    private BeatManager() { }

//--------------SINGLETON --------------------------------------------------------------------------------------------
    public static BeatManager Instance
    {
        get
        {
            // If the instance hasn't been created yet, create it
            if (instance == null)
            {
                instance = new BeatManager();
            }
            return instance;
        }
    }

//------------------------------------------------------------------------------------------------------------------------

    public BeatTrack trackInfo;

    public event Action OnBeat;
    
    private int lastInterval;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var step in trackInfo.Steps) // Time elapsed in beats
        {
            float sampledTime = trackInfo.Track.timeSamples / (trackInfo.Track.clip.frequency * GetIntervalLenght(trackInfo.Bpm, step));
            CheckForNewInterval(sampledTime);
        }
    }

    private float GetIntervalLenght(float bpm, float steps) // The lenght of the current beat
    {
        return 60 / (bpm * steps);
    }

    private void CheckForNewInterval(float interval)
    {
        if(Mathf.FloorToInt(interval)!= lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            OnBeat.Invoke();
        }
    }
}

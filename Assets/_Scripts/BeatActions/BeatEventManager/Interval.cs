using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Interval
{
    public Action OnBeatEvent;

    [SerializeField] private string designation;

    //[field: SerializeField] public bool BeatGrace {  get; private set; }
    //[SerializeField, Range(0.1f, 1f)] private float gracePeriod = 0.5f;
    [SerializeField, Range(0.25f, 4f)] private float steps;
    [SerializeField] private UnityEvent unityTrigger;

    public float BeatProgress { get; private set; }

    private int lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * steps);
    }

    public void CheckForNewInterval(float interval)
    {
        BeatProgress = interval - lastInterval;

        //if(!BeatGrace && interval - lastInterval > 1f - (gracePeriod * steps) / 2)
        //{
        //    BeatGrace = true;
        //}

        if (Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            OnBeatEvent?.Invoke();
            unityTrigger?.Invoke();
        }

        //if (BeatGrace && interval - lastInterval > 0f + (gracePeriod * steps) / 2)
        //{
        //    BeatGrace = false;
        //}
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Interval
{
    public Action OnBeatEvent;

    [field: SerializeField] public bool BeatGrace {  get; private set; }
    [SerializeField, Range(0.3f, 1f)] private float gracePeriod = 0.5f;
    [SerializeField, Range(0.25f, 2f)] private float steps;
    [SerializeField] private UnityEvent unityTrigger;
    
    private int lastInterval;

    public Interval() 
    {
        gracePeriod = (gracePeriod * steps) / 2;
    }

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if(!BeatGrace && interval - lastInterval > 1f - gracePeriod)
        {
            BeatGrace = true;
        }

        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            OnBeatEvent?.Invoke();
            unityTrigger?.Invoke();
        }

        if (BeatGrace && interval - lastInterval > 0f + gracePeriod)
        {
            BeatGrace = false;
        }
    }
}

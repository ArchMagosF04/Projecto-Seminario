using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Interval
{
    [SerializeField] private bool OnBeat;
    [SerializeField, Range(0.1f, 0.5f)] private float gracePeriod = 0.2f;
    [SerializeField, Range(0.25f, 2f)] private float steps;
    [SerializeField] private UnityEvent unityTrigger;
    private Action trigger;
    private int lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if(!OnBeat && interval - lastInterval > 1f - ((gracePeriod * steps) / 2))
        {
            OnBeat = true;
        }

        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            trigger?.Invoke();
            unityTrigger?.Invoke();
        }

        if (OnBeat && interval - lastInterval > 0f + ((gracePeriod * steps) / 2))
        {
            OnBeat = false;
        }
    }
}

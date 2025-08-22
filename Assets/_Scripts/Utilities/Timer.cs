using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public event Action OnTimerDone;

    public float StartTime {  get; private set; }
    public float duration { get; private set; }
    public float TargetTime { get; private set; }

    private bool isActive;

    //public Timer(float duration)
    //{
    //    this.duration = duration;
    //}

    public void StartTimer(float length)
    {
        duration = length;

        StartTime = Time.time;
        TargetTime = StartTime + duration;
        isActive = true;
    }

    public void StopTimer()
    {
        isActive = false;
    }

    public void Tick()
    {
        if (!isActive) return;

        if(Time.time >= TargetTime)
        {
            OnTimerDone?.Invoke();
            StopTimer();
        }
    }
}

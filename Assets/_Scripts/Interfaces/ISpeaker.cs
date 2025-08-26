using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeaker 
{
    public void StartSpeaking();

    public void StopSpeaking();

    public float GetHealth();
}

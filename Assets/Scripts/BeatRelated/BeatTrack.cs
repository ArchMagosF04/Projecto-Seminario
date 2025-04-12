using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Beat Track", menuName = "Beat Track")]
public class BeatTrack : ScriptableObject
{
    [SerializeField] private float bpm;
    public float Bpm { get { return bpm; } }
    [SerializeField] private AudioSource track;
    public AudioSource Track { get { return track; } }

    [SerializeField] private float[] steps;
    public float[] Steps { get { return steps; } }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Track Info", menuName = "Track Info")]
public class TrackInfo : ScriptableObject
{
    [SerializeField] private AudioClip musicTrack;
    public AudioClip MusicTrack {  get { return musicTrack; } }

    [SerializeField] private float bpm;
    public float Bpm { get { return bpm; } }

}

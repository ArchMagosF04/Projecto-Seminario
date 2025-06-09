using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatIndicator : MonoBehaviour
{
    //BeatMarkers
    [SerializeField] private BeatMarker marker;

    private Vector3 markerStartPos;

    private void Start()
    {
        float SPB = BeatManager.Instance.BPM / 60f;
        markerStartPos = transform.position + new Vector3(8f * SPB, 0, 0);
        //SpawnInitialMarkers();
    }

    private void OnEnable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent += SpawnNewMarker;
    }

    private void OnDisable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent -= SpawnNewMarker;
    }

    private void SpawnInitialMarkers()
    {
        for (int i = 0; i < 3; i++)
        {
            BeatMarker newMarker = Instantiate(marker, markerStartPos, Quaternion.identity, transform);
        }
    }

    private void SpawnNewMarker()
    {
        BeatMarker newMarker = Instantiate(marker, markerStartPos, Quaternion.identity, transform);
    }
}

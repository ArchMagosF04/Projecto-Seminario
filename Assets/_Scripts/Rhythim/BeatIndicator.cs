using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatIndicator : MonoBehaviour
{
    //BeatHeart
    [SerializeField] private Transform beatHeart;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField] private float pulseSize = 1.2f;
    private Vector3 startSize;

    //BeatMarkers
    [SerializeField] private BeatMarker marker;

    private void Start()
    {
        startSize = beatHeart.localScale;
        //SpawnInitialMarkers();
    }

    private void Update()
    {
        if (beatHeart.localScale != startSize) beatHeart.localScale = Vector3.Lerp(beatHeart.localScale, startSize, Time.deltaTime * returnSpeed);
    }

    private void OnEnable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent += BeatPulse;
        //BeatManager.Instance.OneBeat.OnBeatEvent += SpawnNewMarker;
    }

    private void OnDisable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent -= BeatPulse;
        //BeatManager.Instance.OneBeat.OnBeatEvent -= SpawnNewMarker;
    }

    public void BeatPulse()
    {
        beatHeart.localScale = startSize * pulseSize;
    }

    private void SpawnInitialMarkers()
    {
        for (int i = 0; i < 3; i++)
        {
            BeatMarker newMarker = Instantiate(marker, transform);
            newMarker.transform.position += new Vector3(2f * i+1, 0f, 0f);
        }
    }

    private void SpawnNewMarker()
    {
        BeatMarker newMarker = Instantiate(marker, transform);
        newMarker.transform.position += new Vector3(2f * 4, 0f, 0f);
    }
}

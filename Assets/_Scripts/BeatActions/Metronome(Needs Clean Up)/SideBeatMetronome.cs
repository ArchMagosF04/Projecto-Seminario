using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class SideBeatMetronome : MonoBehaviour
{
    //Note for working on this: Beat Manager does not start with a beat, it will wait to call the first event in the sequence.

    [Header("Marker Components")]
    [SerializeField] private Transform markerSpawnPoint;
    [SerializeField] private MetronomeMarker markerPrefab;

    [Header("Marker Lists")]
    [SerializeField] private List<MetronomeMarker> activeMarkers = new List<MetronomeMarker>();
    [SerializeField] private List<MetronomeMarker> reserveMarkers = new List<MetronomeMarker>();

    private Animator anim;

    [SerializeField] private Color colorOnBeatGrace;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        foreach (var marker in activeMarkers) marker.ToggleMovility(true);
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent += MetronomeFullBeat;
        //BeatManager.Instance.OnWrongBeat += OnBeatMiss;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= MetronomeFullBeat;
        //BeatManager.Instance.OnWrongBeat -= OnBeatMiss;
    }

    private MetronomeMarker GetMarker()
    {
        MetronomeMarker marker;

        if (reserveMarkers.Count > 0)
        {
            marker = reserveMarkers[0];
            reserveMarkers.RemoveAt(0);
        }
        else
        {
            marker = Instantiate(markerPrefab, markerSpawnPoint);
        }

        marker.gameObject.SetActive(false);
        activeMarkers.Add(marker);
        marker.ChangeMarkerColor(Color.white);

        return marker;
    }

    private void MetronomeFullBeat()
    {
        //ResetMarkers();

        anim.SetTrigger("Beat");

        MetronomeMarker newMarker = GetMarker();
        newMarker.gameObject.SetActive(true);
        newMarker.transform.position = markerSpawnPoint.position;
        newMarker.ToggleMovility(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BeatManager.Instance.ToggleGracePeriod(true);

        if (collision.TryGetComponent<MetronomeMarker>(out MetronomeMarker marker)) marker.ChangeMarkerColor(colorOnBeatGrace);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BeatManager.Instance.ToggleGracePeriod(false);

        if (collision.TryGetComponent<MetronomeMarker>(out MetronomeMarker marker))
        {
            if (activeMarkers.Contains(marker))
            {
                marker.ToggleMovility(false);
                activeMarkers.Remove(marker);
                reserveMarkers.Add(marker);
                marker.gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinalBeatMetronome : MonoBehaviour
{
    [Header("Other Components")]
    [SerializeField] private GameObject OnBeatIndicator;

    [Header("Marker Components")]
    [SerializeField] private Transform markerSpawnPoint;
    [SerializeField] private MetronomeBarMarker markerPrefab;

    [Header("Marker Lists")]
    [SerializeField] private List<MetronomeBarMarker> activeMarkers = new List<MetronomeBarMarker>();
    [SerializeField] private List<MetronomeBarMarker> reserveMarkers = new List<MetronomeBarMarker>();

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        OnBeatIndicator.SetActive(false);
    }

    private void Start()
    {
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
        foreach (var marker in activeMarkers) marker.ToggleMovility(true);
    }

    private void MetronomeFullBeat()
    {
        anim.SetTrigger("Beat");

        MetronomeBarMarker newMarker = GetMarker();
        newMarker.gameObject.SetActive(true);
        newMarker.transform.position = markerSpawnPoint.position;
        newMarker.ToggleMovility(true);
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent += MetronomeFullBeat;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= MetronomeFullBeat;
    }

    private MetronomeBarMarker GetMarker()
    {
        MetronomeBarMarker marker;

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

        return marker;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BeatManager.Instance.ToggleGracePeriod(true);

        OnBeatIndicator.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MetronomeBarMarker>(out MetronomeBarMarker marker))
        {
            if (activeMarkers.Contains(marker))
            {
                marker.ToggleMovility(false);
                activeMarkers.Remove(marker);
                reserveMarkers.Add(marker);
                marker.gameObject.SetActive(false);
                OnBeatIndicator.SetActive(false);
            }
        }

        BeatManager.Instance.ToggleGracePeriod(false);
    }
}

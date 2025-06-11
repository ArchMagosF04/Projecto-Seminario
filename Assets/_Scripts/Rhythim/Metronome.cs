using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private Transform markerSpawnPoint;
    [SerializeField] private float markerSpeed = 2f;
    [SerializeField] private float numberOfMarkers = 5f;

    private List<GameObject> markers = new List<GameObject>();

    private Vector3 markerSpawnlocation;
    private Vector3 markerOffset;

    private float timelapse;

    private void Start()
    {
        float distance = markerSpeed * (60f / BeatManager.Instance.BPM);
        markerOffset = new Vector3(distance, 0f, 0f);
        markerSpawnlocation = transform.position + markerOffset * numberOfMarkers;
        markerSpawnPoint.position = markerSpawnlocation;
        timelapse = Time.time;
        SpawnInitialMarkers();
    }

    private void Update()
    {
        foreach (var marker in markers)
        {
            float xPos = Time.deltaTime * - markerSpeed + marker.transform.position.x;
            marker.transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }

    private void OnEnable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent += SpawnMarker;
    }

    private void OnDisable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent -= SpawnMarker;
    }

    private void SpawnMarker()
    {
        //float timebetweenBeats = Time.time - timelapse;
        //Debug.Log(timebetweenBeats);
        //timelapse = Time.time;

        //if (markers.Count > 0) Debug.Log(markers[0].transform.position.x);

        GameObject newMarker = Instantiate(markerPrefab, markerSpawnPoint.position, Quaternion.identity, transform);
        markers.Add(newMarker);
    }

    private void SpawnInitialMarkers()
    {
        for (int i = 0; i < numberOfMarkers - 1; i++)
        {
            Vector3 spawnLocation = transform.position + markerOffset * i;
            GameObject newMarker = Instantiate(markerPrefab, spawnLocation, Quaternion.identity, transform);
            markers.Add(newMarker);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BeatMarker"))
        {
            if(markers.Contains(collision.gameObject))
            {
                BeatManager.Instance.ToggleGracePeriod(true);
                SpriteRenderer sprite = collision.GetComponent<SpriteRenderer>();
                sprite.color = Color.red;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BeatMarker"))
        {
            if (markers.Contains(collision.gameObject))
            {
                BeatManager.Instance.ToggleGracePeriod(false);
                markers.Remove(collision.gameObject);
                Destroy(collision.gameObject);
            }
        }
    }

}

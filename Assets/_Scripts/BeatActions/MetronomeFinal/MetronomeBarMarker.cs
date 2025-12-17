using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeBarMarker : MonoBehaviour
{
    private bool isMoving;

    [Header("MovementParameters")]
    [SerializeField] private float speed = 0;
    [SerializeField] private float distance = 1.875f;
    [SerializeField] private float timePerBeat;

    private float lastSampleTime;
    private float deltaInterval;

    private void Start()
    {
        timePerBeat = 60 / BeatManager.Instance.BPM;
        speed = distance / timePerBeat;
    }

    private void OnEnable()
    {
        isMoving = false;
    }

    private void Update()
    {
        CalculateSampleTime();

        if (isMoving)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    private void CalculateSampleTime()
    {
        float currentSampleTime = BeatManager.Instance.sampledTime;
        deltaInterval = currentSampleTime - lastSampleTime;
        lastSampleTime = currentSampleTime;
    }

    public void ToggleMovility(bool toggle) => isMoving = toggle;
}

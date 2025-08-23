using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
    public float MovementSpeed { get; private set; }

    [SerializeField] private float speedMultiplier;

    private float beatSpeed;

    private void Awake()
    {
        RB = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start()
    {
        beatSpeed = BeatManager.Instance.BeatSpeedMultiplier;
        SetPlatformSpeed();
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[4].OnBeatEvent += ReverseSpeed;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[4].OnBeatEvent -= ReverseSpeed;
    }

    private void ReverseSpeed()
    {
        speedMultiplier *= -1;
        SetPlatformSpeed();
    }

    public void SetPlatformSpeed()
    {
        MovementSpeed = speedMultiplier * beatSpeed;
        RB.velocity = Vector2.right * MovementSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private float speedMultiplier;

    private Rigidbody2D rb;

    private float beatSpeed;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start()
    {
        beatSpeed = BeatManager.Instance.BeatSpeedMultiplier;
        //SetPlatformSpeed();
    }

    private void Update()
    {
        transform.position += new Vector3(beatSpeed * speedMultiplier, 0) * Time.deltaTime;
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
        //SetPlatformSpeed();
    }

    public void SetPlatformSpeed()
    {
        rb.velocity = Vector2.right * speedMultiplier * beatSpeed;
    }
}

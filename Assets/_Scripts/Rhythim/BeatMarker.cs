using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMarker : MonoBehaviour
{
    private float beatsPerSecond;
    private SpriteRenderer spriteRenderer;

    private bool gracePeriod = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        beatsPerSecond = BeatManager.Instance.BPM / 60f;
    }

    private void OnEnable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent += Talk;
    }

    private void OnDisable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent -= Talk;
    }

    private void Update()
    {
        transform.position -= new Vector3(2f * beatsPerSecond * Time.deltaTime, 0f, 0f);
        //if (transform.localPosition.x <= 0f)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void Talk()
    {
        if (gracePeriod) Debug.Log(transform.position.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BeatCenter"))
        {
            spriteRenderer.color = Color.blue;
            gracePeriod = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BeatCenter"))
        {
            gracePeriod = false;
            Destroy(gameObject);
        }
    }
}

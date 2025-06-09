using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseToBeat : MonoBehaviour
{
    [SerializeField] private bool useTestBeat;
    [SerializeField] private float pulseSize = 1.2f;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField] private bool signalGracePeriod;

    private Vector3 startSize;
    private SpriteRenderer sprite;
    private bool colorBackToNormal = true;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startSize = transform.localScale;
        if(useTestBeat)
        {
            StartCoroutine(TestBeat());
        }
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);

        if (!signalGracePeriod) return;
        if (BeatManager.Instance.OneBeat.BeatGrace)
        {
            sprite.color = Color.white;
            colorBackToNormal = false;
        }
        else if (!BeatManager.Instance.OneBeat.BeatGrace && !colorBackToNormal)
        {
            sprite.color = Color.black;
            colorBackToNormal = true;
        }
    }

    public void Pulse()
    {
        transform.localScale = startSize * pulseSize;
    }

    private IEnumerator TestBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Pulse();
        }
    }
}

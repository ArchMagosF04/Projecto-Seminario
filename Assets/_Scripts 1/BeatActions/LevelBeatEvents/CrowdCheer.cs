using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdCheer : MonoBehaviour
{
    [SerializeField] private float riseDistance = 1.2f;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField, Range(0f, 1f)] private float cheerDuration = 0.15f;

    [SerializeField] private GameObject cheerSprite;

    private Vector3 startPos;

    private Coroutine returnToStart;

    private void Start()
    {
        startPos = transform.position;
        cheerSprite.SetActive(false);
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent += Rise;
        BeatManager.Instance.OnCorrectBeat += Cheer;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= Rise;
        BeatManager.Instance.OnCorrectBeat -= Cheer;
    }
    public void Rise()
    {
        if (returnToStart != null) StopCoroutine(returnToStart);

        transform.position += new Vector3(0f, riseDistance, 0f);
        returnToStart = StartCoroutine(ReturnToOriginalPos());
    }

    public void Cheer()
    {
        cheerSprite.SetActive(true);
        StartCoroutine(DisableCheer());
    }

    private IEnumerator DisableCheer()
    {
        yield return new WaitForSeconds(cheerDuration);
        cheerSprite.SetActive(false);
    }

    private IEnumerator ReturnToOriginalPos()
    {
        while (transform.position != startPos)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * returnSpeed);
            yield return null;
        }
    }
}

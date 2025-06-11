using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdCheer : MonoBehaviour
{
    [SerializeField] private float riseDistance = 1.2f;
    [SerializeField] private float returnSpeed = 5f;

    [SerializeField] private GameObject cheerSprite;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        cheerSprite.SetActive(false);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * returnSpeed);
    }

    private void OnEnable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent += Rise;
        BeatManager.Instance.OnCorrectBeat += Cheer;
    }

    private void OnDisable()
    {
        BeatManager.Instance.OneBeat.OnBeatEvent -= Rise;
        BeatManager.Instance.OnCorrectBeat -= Cheer;
    }
    public void Rise()
    {
        transform.position += new Vector3(0f, riseDistance, 0f);
    }

    public void Cheer()
    {
        cheerSprite.SetActive(true);
        StartCoroutine(DisableCheer());
    }

    private IEnumerator DisableCheer()
    {
        yield return new WaitForSeconds(0.15f);
        cheerSprite.SetActive(false);
    }
}

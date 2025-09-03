using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidTrigger : MonoBehaviour
{
    private Collider2D objectCollider;

    private void Start()
    {
        objectCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Solidify());
    }

    IEnumerator Solidify()
    {
        yield return new WaitForSecondsRealtime(1);

        objectCollider.isTrigger = false;

        yield return null;
    }
}

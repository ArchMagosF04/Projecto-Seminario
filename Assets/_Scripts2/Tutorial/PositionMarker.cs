using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMarker : MonoBehaviour
{
    [SerializeField] GameObject nextMarker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nextMarker != null)
        {
            nextMarker.SetActive(true);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

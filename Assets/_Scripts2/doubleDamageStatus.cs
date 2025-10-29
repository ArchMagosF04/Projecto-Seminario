using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doubleDamageStatus : MonoBehaviour
{
    public float duration = 3;

    private void Start()
    {
        GetComponentInParent<Core_Health>().doubleDamage = true;
        Destroy(gameObject, duration);
    }

    private void OnDestroy()
    {
        GetComponentInParent<Core_Health>().doubleDamage = false;
    }

    private void OnDisable()
    {
        GetComponentInParent<Core_Health>().doubleDamage = false;
    }
}

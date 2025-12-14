using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerZone : MonoBehaviour
{
    [SerializeField] private bool destroyOnEntering;

    [Header("Events")]
    [SerializeField] private UnityEvent onTriggerEnter;
    [SerializeField] private UnityEvent onTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter?.Invoke();

        if (destroyOnEntering) Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit?.Invoke();
    }
}

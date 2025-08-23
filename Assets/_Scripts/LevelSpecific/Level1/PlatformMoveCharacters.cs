using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformMoveCharacters : MonoBehaviour
{
    [SerializeField] private List<Rigidbody2D> objectsOnPlatform = new List<Rigidbody2D>();

    private MovingPlatforms platform;

    private void Awake()
    {
        platform = GetComponentInParent<MovingPlatforms>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            if (!objectsOnPlatform.Contains(rb)) objectsOnPlatform.Add(rb);
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            if (objectsOnPlatform.Contains(rb)) objectsOnPlatform.Remove(rb);
        }
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D rb in objectsOnPlatform)
        {
            if (Mathf.Abs(rb.velocityX) <= 0.05f || Mathf.Abs(rb.velocityX) == Mathf.Abs(platform.MovementSpeed))
            {
                rb.velocityX = platform.MovementSpeed;
            }
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLaneFloor : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce;

    [SerializeField] protected ScreenShakeProfile shakeProfile;

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Core_Knockback component))
        {
            component.Knockback(Vector2.up, knockbackForce, 1);
            CameraShakeManager.Instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
        }

        if (collision.TryGetComponent(out Core_Health health))
        {
            health.TakeDamage(damage, transform.right);
        }
    }
}

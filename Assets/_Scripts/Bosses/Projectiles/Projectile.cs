using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private float speed;

    [SerializeField] private float lifeTime = 5f;

    [SerializeField] private ScreenShakeProfile shakeProfile;

    private Rigidbody2D rb;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void LaunchProjectile(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Core_Knockback component))
        {
            component.Knockback(transform, knockback);
            CameraShakeManager.Instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
        }

        if (collision.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(damage, transform.right);
        }

        Destroy(gameObject);
    }
}

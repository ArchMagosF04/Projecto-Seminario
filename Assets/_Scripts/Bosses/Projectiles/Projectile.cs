using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private float speed;

    private Vector2 moveDirection;

    [SerializeField] private float lifeTime = 5f;

    [SerializeField] private ScreenShakeProfile shakeProfile;

    private Rigidbody2D rb;
    private CinemachineImpulseSource impulseSource;
    public bool destroy = true;

    public event System.Action OnHit = delegate { };

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
        moveDirection = direction;
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hadEffect = false;

        if (collision.TryGetComponent(out Core_Knockback component))
        {
            component.Knockback(transform, knockback);
            
            if (!component.HyperArmor)
            {
                hadEffect = true;
                CameraShakeManager.Instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
            }
        }

        if (collision.TryGetComponent(out Core_Health health))
        {
            health.TakeDamage(damage, transform.right);
            if (!health.Invincible) hadEffect = true;
            OnHit();
        }

        if (hadEffect) Destroy(gameObject);      
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float amount)
    {
        speed = amount;
    }

    public Vector2 GetDirection()
    {
        return moveDirection;
    }

    private void OnDestroy()
    {
        OnHit = null;
    }
}

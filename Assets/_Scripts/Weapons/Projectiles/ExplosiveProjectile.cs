using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ExplosiveProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Vector2 speed;
    private Vector2 workSpace;

    [SerializeField] private float explosionRadius;

    [SerializeField] private float damage;

    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void FireProjectile(int direction)
    {
        workSpace.Set(speed.x * direction, speed.y);
        rb.velocity = workSpace;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("Active", true);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    private void InflictDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask);

        foreach (var item in colliders.ToList())
        {
            if (item.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void BulletDeath()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

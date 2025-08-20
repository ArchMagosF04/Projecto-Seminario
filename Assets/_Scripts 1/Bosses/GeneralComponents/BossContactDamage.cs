using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossContactDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Core_Knockback component))
        {
            component.Knockback(transform, knockbackForce);
        }

        if (collision.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(damage, transform.right);
        }
    }
}

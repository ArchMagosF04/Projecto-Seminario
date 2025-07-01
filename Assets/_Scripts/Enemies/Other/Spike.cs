using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        if (collision.TryGetComponent<IKnockBackable>(out IKnockBackable knockbackable))
        {
            int direction = -1;
            if (collision.transform.position.x > transform.position.x) direction = 1;

            knockbackable.KnockBack(new Vector2(2f, 3f), 15f, direction);
        }
    }
}

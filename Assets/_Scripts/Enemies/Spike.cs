using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(10f);
        }

        if (collision.TryGetComponent<IKnockBackable>(out IKnockBackable knockbackable))
        {
            knockbackable.KnockBack(new Vector2(2f, 3f), 15f, 1);
        }
    }
}

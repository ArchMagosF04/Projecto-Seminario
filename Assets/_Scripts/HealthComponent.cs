using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]private float health = 0;

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            Debug.Log(damage);
        }

    }

    public float GetHealth()
    {
        return health;
    }

    public void AssignHealth(float maxHealth)
    {
        if (health == 0)
        {
            health = maxHealth;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]private float health = 0;
    [SerializeField] private Image healthbar;

    public System.Action OnDeath = delegate { };
    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            Debug.Log(damage);
            healthbar.fillAmount -= damage/100;
        }
        else if (health <= 0)
        {
            OnDeath();
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

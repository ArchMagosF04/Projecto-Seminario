using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]private float health = 0;
    [SerializeField] private float maxHealth;
    [SerializeField] private Image healthbar;

    public System.Action OnDeath = delegate { };
    private void Start()
    {
        healthbar.fillAmount = health;
    }

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

    public void AssignHealth(float amount)
    {
        if (health == 0)
        {
            health = amount;
            maxHealth = amount;
            healthbar.fillAmount = health/100;
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        healthbar.fillAmount = health / 100;
    }
}

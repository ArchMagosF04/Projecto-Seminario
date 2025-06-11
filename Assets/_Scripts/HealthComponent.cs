using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]private float health = 0;
    [SerializeField] private Image healthbar;

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            Debug.Log(damage);
            healthbar.fillAmount -= damage/100;
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

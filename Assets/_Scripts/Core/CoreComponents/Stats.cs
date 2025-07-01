using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : CoreComponent
{
    public event Action OnHealthZero;

    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
        if (healthBar != null) healthBar.fillAmount = currentHealth/maxHealth;
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth = MathF.Round(currentHealth - amount);

        if (healthBar != null) healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnHealthZero?.Invoke();
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth +  amount, 0, maxHealth);
        if (healthBar != null) healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
    }
}

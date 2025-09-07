using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Core_Health : CoreComponent, IDamageable
{
    [Header("UI References")]
    [SerializeField] private Image healthBar;
    //[SerializeField] private TextMeshProUGUI healthNumber;
    [SerializeField] private DamagePopup popupPrefab;

    [Header("Stats")]
    [SerializeField] private float maxHealth;
    public float CurrentHealth {  get; private set; }
    public float MaxHealth => maxHealth;

    [Header("Particle Prefabs")]
    [SerializeField] private ParticleSystem damageParticles;

    [Header("Events")]
    public Action OnDeath;
    public Action OnDamageReceived;
    public UnityEvent OnDeathUN;
    public UnityEvent OnDamageReceivedUN;

    public bool Invincible { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Invincible = false;

        CurrentHealth = maxHealth;

        if (healthBar != null) healthBar.fillAmount = CurrentHealth / maxHealth;
        //if (healthNumber != null) healthNumber.text = $"{currentHealth} / {maxHealth}";
    }

    public void ToggleInvincibility(bool input) => Invincible = input;

    public void TakeDamage(float amount, Vector2 attackDirection)
    {
        CurrentHealth = MathF.Round(CurrentHealth - amount);

        OnDamageReceived?.Invoke();
        OnDamageReceivedUN?.Invoke();

        SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("DamageSound")).Play();

        if (popupPrefab != null)
        {
            DamagePopup popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
            popup.Setup((int)MathF.Round(amount));
        }

        if (damageParticles != null)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
            Instantiate(damageParticles, transform.position, rotation);
        }


        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
            OnDeathUN?.Invoke();
        }

        if (healthBar != null) healthBar.fillAmount = CurrentHealth / maxHealth;
    }

    public void HealHealth(float amount)
    {
        CurrentHealth += amount;

        CurrentHealth = Mathf.Round(CurrentHealth);

        if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;

        if (healthBar != null) healthBar.fillAmount = CurrentHealth / maxHealth;
        //if (healthNumber != null) healthNumber.text = $"{currentHealth} / {maxHealth}";
    }

    [ContextMenu("Damage Test")]
    public void DamageTest()
    {
        TakeDamage(10, Vector2.right);
    }

    [ContextMenu("Heal Test")]
    public void HealHealthTest()
    {
        HealHealth(10);
    }
}

using Ami.BroAudio;
using Microlight.MicroBar;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Core_Health : CoreComponent
{
    [Header("UI References")]
    [SerializeField] private Image healthBar;
    [SerializeField] private MicroBar microBar;
    //[SerializeField] private TextMeshProUGUI healthNumber;
    //[SerializeField] private DamagePopup popupPrefab;

    [Header("Stats")]
    [SerializeField] private float maxHealth;
    public float CurrentHealth {  get; private set; }
    public float MaxHealth => maxHealth;

    [Header("Invincibility Frames")]
    [SerializeField] private bool hasIFrames = false;
    [SerializeField, Range(0f, 1f)] private float iFramesDuration;

    [Header("Particle Prefabs")]
    [SerializeField] private ParticleSystem damageParticles;

    [Header("Sounds")]
    [SerializeField] private SoundID damageSound;

    [Header("Events")]
    public Action OnDeath;
    public Action OnDamageReceived;
    public Action OnInvincibleHit;
    public UnityEvent OnDeathUN;
    public UnityEvent OnDamageReceivedUN;
    public UnityEvent OnInvincibleHitUN;

    public bool Invincible { get; private set; }
    public bool doubleDamage;

    protected override void Awake()
    {
        CurrentHealth = maxHealth;
        base.Awake();
    }

    private void Start()
    {
        Invincible = false;

        if (healthBar != null) healthBar.fillAmount = CurrentHealth / maxHealth;
        if (microBar != null) microBar.Initialize(maxHealth);
        //if (healthNumber != null) healthNumber.text = $"{currentHealth} / {maxHealth}";
    }

    public void ToggleInvincibility(bool input) => Invincible = input;

    public void TakeDamage(float amount, Vector2 attackDirection)
    {
        if (Invincible)
        {
            OnInvincibleHit?.Invoke();
            OnInvincibleHitUN?.Invoke();
            return;
        }

        if (doubleDamage) amount = amount * 2;

        CurrentHealth = MathF.Round(CurrentHealth - amount);

        Debug.Log(CurrentHealth, this);

        OnDamageReceived?.Invoke();
        OnDamageReceivedUN?.Invoke();

        if (damageSound.IsValid()) BroAudio.Play(damageSound);

        //if (popupPrefab != null)
        //{
        //    DamagePopup popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
        //    popup.Setup((int)MathF.Round(amount));
        //}

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
        if (microBar != null) microBar.UpdateBar(CurrentHealth, UpdateAnim.Damage);

        if (hasIFrames)
        {
            ToggleInvincibility(true);
            StopAllCoroutines();
            StartCoroutine(IFramesTimer());
        }
    }

    public void HealHealth(float amount)
    {
        CurrentHealth += amount;

        CurrentHealth = Mathf.Round(CurrentHealth);

        if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;

        if (healthBar != null) healthBar.fillAmount = CurrentHealth / maxHealth;
        if (microBar != null) microBar.UpdateBar(CurrentHealth, UpdateAnim.Heal);
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

    private IEnumerator IFramesTimer()
    {
        yield return new WaitForSeconds(iFramesDuration);
        ToggleInvincibility(false);
    }

    public float GetCurrentHealthPercentage()
    {
        return CurrentHealth / maxHealth;
    }
}

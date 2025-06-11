using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageReceiver : CoreComponent, IDamageable
{
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private Image healthBar;

    public bool IsInvincible { get; private set; }

    private CoreComp<Stats> stats;
    private CoreComp<ParticleManager> particleManager;

    protected override void Awake()
    {
        base.Awake();

        stats = new CoreComp<Stats>(core);
        particleManager = new CoreComp<ParticleManager>(core);
    }

    public void ToggleInvincibility(bool value)
    {
        IsInvincible = value;
    }

    public void TakeDamage(float amount)
    {
        if (IsInvincible) return;

        Debug.Log(core.transform.parent.name + " Damaged");

        stats.Comp?.DecreaseHealth(amount);
        healthBar.fillAmount -= amount;

        //particleManager.Comp?.StartParticlesWithRandomRotation(damageParticles);
    }
}

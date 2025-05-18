using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : CoreComponent, IDamageable
{
    [SerializeField] private GameObject damageParticles;

    private Stats stats;
    private ParticleManager particleManager;

    protected override void Awake()
    {
        base.Awake();

        stats = core.GetCoreComponent<Stats>();
        particleManager = core.GetCoreComponent<ParticleManager>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(core.transform.parent.name + " Damaged");

        stats.Health.Decrease(amount);

        //particleManager.StartParticlesWithRandomRotation(damageParticles);
    }
}

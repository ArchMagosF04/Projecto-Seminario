using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : CoreComponent, IDamageable
{
    [SerializeField] private GameObject damageParticles;

    private CoreComp<Stats> stats;
    private CoreComp<ParticleManager> particleManager;

    protected override void Awake()
    {
        base.Awake();

        stats = new CoreComp<Stats>(core);
        particleManager = new CoreComp<ParticleManager>(core);
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(core.transform.parent.name + " Damaged");

        stats.Comp?.DecreaseHealth(amount);

        //particleManager.Comp?.StartParticlesWithRandomRotation(damageParticles);
    }
}

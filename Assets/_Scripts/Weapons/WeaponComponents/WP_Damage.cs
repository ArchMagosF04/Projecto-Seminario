using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WP_Damage : WeaponComponent
{
    private WP_Hitbox hitbox;

    protected override void Awake()
    {
        base.Awake();

        hitbox = GetComponentInChildren<WP_Hitbox>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventHandler.OnAttackAction += HandleDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventHandler.OnAttackAction -= HandleDamage;
    }

    private void HandleDamage()
    {
        AD_Damage data = WeaponData.DamageData[weapon.CurrentAttackCounter];
        float multipler = 1f;
        if (weapon.isOnBeat)
        {
            multipler = data.OnBeatMultipler;
            Debug.Log("WeaponOnBeat");
        }

        if (weapon.isSpecialAttack) data = WeaponData.SpecialDamageData;

        foreach (var item in hitbox.collider2Ds.ToList())
        {
            if (item.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(data.Amount * multipler);
            }

            if(item.TryGetComponent(out HealthComponent healthComponent))
            {
                healthComponent.TakeDamage(data.Amount);
            }
        }
    }


}

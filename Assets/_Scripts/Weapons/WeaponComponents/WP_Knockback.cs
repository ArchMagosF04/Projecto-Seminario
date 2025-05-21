using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WP_Knockback : WeaponComponent
{
    private Movement coreMovement;
    private Movement CoreMovement => coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);

    private WP_Hitbox hitbox;

    protected override void Awake()
    {
        base.Awake();

        hitbox = GetComponentInChildren<WP_Hitbox>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventHandler.OnAttackAction += HandleKnockback;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventHandler.OnAttackAction -= HandleKnockback;
    }

    private void HandleKnockback()
    {
        AD_Knockback data = WeaponData.KnockbackData[weapon.CurrentAttackCounter];
        if (weapon.isSpecialAttack) data = WeaponData.SpecialKnockbackData;

        foreach (var item in hitbox.collider2Ds.ToList())
        {
            if (item.TryGetComponent(out IKnockBackable knockBackable))
            {
                knockBackable.KnockBack(data.Angle, data.Strength, CoreMovement.FacingDirection);
            }
        }
    }
}

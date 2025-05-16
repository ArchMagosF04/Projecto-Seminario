using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnockBack : WeaponComponent<WeaponKnockBackData, AttackKnockBack>
{
    private WeaponActionHitBox hitBox;

    private Movement movement;

    protected override void Start()
    {
        base.Start();

        movement = core.GetCoreComponent<Movement>();

        hitBox = GetComponent<WeaponActionHitBox>();

        hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
    }

    protected void HandleDetectCollider2D(Collider2D[] colliders)
    {
        foreach(var item in colliders)
        {
            if(item.TryGetComponent(out IKnockBackable knockBackable))
            {
                knockBackable.KnockBack(currentAttackData.Angle, currentAttackData.Strength, movement.FacingDirection);
            }
        }
    }
}

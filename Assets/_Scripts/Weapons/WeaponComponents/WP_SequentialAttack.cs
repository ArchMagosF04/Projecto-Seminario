using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_SequentialAttack : WeaponComponent
{
    protected override void OnEnable()
    {
        base.OnEnable();
        weapon.OnExit += IncreaseAttackCounter;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        weapon.OnExit -= IncreaseAttackCounter;
    }

    private void IncreaseAttackCounter()
    {
        weapon.ModifyAttackCounter(weapon.CurrentAttackCounter + 1);
    }
}

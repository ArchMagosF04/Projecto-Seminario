using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirectionalWeapon : AggressiveWeapon
{
    private int directionalAttackIndex; //0 = Forward, 1 = Up, 2 = Down, 3 = UpForward, 4 = DownForward.

    private Vector2 workSpace;

    private Vector2[] directions;

    protected override void Awake()
    {
        base.Awake();

        directions = new Vector2[5];

        directions[0] = new Vector2(0, 0);
        directions[1] = new Vector2(0, 1);
        directions[2] = new Vector2(0, -1);
        directions[3] = new Vector2(1, 1);
        directions[0] = new Vector2(1, -1);
    }

    public override void EnterWeapon()
    {
        base.EnterWeapon();

        weaponAnimator.SetInteger("xInput", state.XInput);
        weaponAnimator.SetInteger("yInput", state.YInput);
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }

    protected override void CheckMeleeAttack()
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[CheckDirection()];

        foreach (IDamageable item in detectedDamageables.ToList()) //Creates a copy of the list so the original one can be modified by destroying a member.
        {
            item.TakeDamage(details.damageAmount);
        }

        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(details.knockbackAngle, details.knockbackStrength, Movement.FacingDirection);
        }
    }

    private int CheckDirection()
    {
        workSpace.Set(state.XInput, state.YInput);

        for(int i = 0; i < directions.Length; i++)
        {
            if (directions[i] == workSpace) return i;
        }

        return 0;
    }
}

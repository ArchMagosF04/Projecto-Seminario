using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    protected List<IDamageable> detectedDamageables = new List<IDamageable>();
    protected List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();

    protected override void Awake()
    {
        base.Awake();

        if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for the weapon");
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }

    protected virtual void CheckMeleeAttack()
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];

        foreach (IDamageable item in detectedDamageables.ToList()) //Creates a copy of the list so the original one can be modified by destroying a member.
        {
            item.TakeDamage(details.damageAmount);
        }

        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(details.knockbackAngle, details.knockbackStrength, Movement.FacingDirection);
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            detectedDamageables.Add(damageable);
            //Debug.Log("AddToDetected");
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            detectedKnockbackables.Add(knockbackable);
            //Debug.Log("AddToDetected");
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            detectedDamageables.Remove(damageable);
            //Debug.Log("RemoveFromDetected");
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            detectedKnockbackables.Remove(knockbackable);
            //Debug.Log("AddToDetected");
        }
    }
}

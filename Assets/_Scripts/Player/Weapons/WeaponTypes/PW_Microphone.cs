using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PW_Microphone : PlayerWeapon
{
    [Header("Weapon Stats")]
    [SerializeField] protected float basicAttackDamage = 3f;
    [SerializeField] protected float specialAttackDamage = 1f;
    [SerializeField] protected float manaOnBeatHit;

    private MeleeWeaponHitbox hitbox;

    protected override void Awake()
    {
        base.Awake();
        hitbox = GetComponentInChildren<MeleeWeaponHitbox>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.OnAttackAction += BasicAttackDamage;
        OnSpecialEnter += SpecialAttackDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.OnAttackAction -= BasicAttackDamage;
        OnSpecialEnter -= SpecialAttackDamage;
        BeatManager.Instance.intervals[2].OnBeatEvent -= SpecialHitOnBeat;
        OnExit -= UnsubFromBeat;
    }

    private void BasicAttackDamage()
    {
        //DealDamage(basicAttackDamage);

        foreach (var item in hitbox.collider2Ds.ToList())
        {
            if (item.TryGetComponent(out IDamageable damageable))
            {
                float multiplier = 0.8f;
                if (isOnBeat) multiplier = beatCombo.currentRank.rankDamageMultiplier;

                damageable.TakeDamage(basicAttackDamage * multiplier, movementComponent.FacingDirection * Vector2.right);

                beatCombo.IncreaseComboCounter();
                if (isOnBeat) manaComponent.IncreaseMana(manaOnBeatHit);
            }
        }
    }

    private void SpecialAttackDamage()
    {
        BeatManager.Instance.intervals[2].OnBeatEvent += SpecialHitOnBeat;
        OnExit += UnsubFromBeat;
    }

    protected void SpecialHitOnBeat()
    {
        foreach (var item in hitbox.collider2Ds.ToList())
        {
            if (item.TryGetComponent(out IDamageable damageable))
            {
                float multiplier = 0.8f;
                if (isOnBeat) multiplier = beatCombo.currentRank.rankDamageMultiplier;

                damageable.TakeDamage(specialAttackDamage * multiplier, movementComponent.FacingDirection * Vector2.right);

                beatCombo.IncreaseComboCounter();
            }
        }

        //DealDamage(specialAttackDamage);
    }

    protected void UnsubFromBeat()
    {
        BeatManager.Instance.intervals[2].OnBeatEvent -= SpecialHitOnBeat;
    }

    private void DealDamage(float damage)
    {
        foreach (var item in hitbox.collider2Ds.ToList())
        {
            if (item.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage * beatCombo.currentRank.rankDamageMultiplier, movementComponent.FacingDirection * Vector2.right);
                beatCombo.IncreaseComboCounter();
            }
        }
    }
}

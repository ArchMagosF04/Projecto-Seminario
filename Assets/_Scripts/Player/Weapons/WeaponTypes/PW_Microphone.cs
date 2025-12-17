using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PW_Microphone : PlayerWeapon
{
    [Header("Weapon Stats")]
    [SerializeField] protected float basicAttackDamage = 3f;
    [SerializeField] protected float specialAttackDamage = 1f;
    [SerializeField, Range(0f, 1f)] private float damgePenalty = 0.1f;
    [SerializeField] protected float manaOnBeatHit;
    [SerializeField] private int comboGain = 1;

    [Header("Sounds")]
    [SerializeField] private SoundID missBeatSound;
    [SerializeField] private SoundID beatHitSound;
    [SerializeField] private SoundID specialHitSound;

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

    public override void ExecuteBasicAttack()
    {
        anim.SetInteger("YInput", GameInputManager.Instance.NormInputY);

        base.ExecuteBasicAttack();
    }

    private void BasicAttackDamage()
    {
        if (isOnBeat)
        {
            if (beatHitSound.IsValid()) BroAudio.Play(beatHitSound);
        }
        else
        {
            if (missBeatSound.IsValid()) BroAudio.Play(missBeatSound);
        }
        foreach (var item in hitbox.collider2Ds.ToList())
        {
            if (item.TryGetComponent(out Core_Health damageable))
            {
                float multiplier = damgePenalty;
                if (isOnBeat)
                {
                    multiplier = beatCombo.GetDamageMultiplier();

                    beatCombo.IncreaseComboCounter(comboGain);
                    manaComponent.IncreaseMana(manaOnBeatHit);
                }

                damageable.TakeDamage(basicAttackDamage * multiplier, movementComponent.FacingDirection * Vector2.right);
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
            if (item.TryGetComponent(out Core_Health damageable))
            {
                float multiplier = 0.1f;
                if (isOnBeat) multiplier = beatCombo.GetDamageMultiplier();

                if (specialHitSound.IsValid()) BroAudio.Play(specialHitSound);

                damageable.TakeDamage(specialAttackDamage * multiplier, movementComponent.FacingDirection * Vector2.right);

                beatCombo.IncreaseComboCounter(comboGain);
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
            if (item.TryGetComponent(out Core_Health damageable))
            {
                damageable.TakeDamage(damage * beatCombo.currentRank.rankDamageMultiplier, movementComponent.FacingDirection * Vector2.right);
                beatCombo.IncreaseComboCounter(comboGain);
            }
        }
    }
}

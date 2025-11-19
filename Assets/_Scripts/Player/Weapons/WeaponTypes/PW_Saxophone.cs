using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_Saxophone : PlayerWeapon
{
    [Header("Weapon Stats")]
    [SerializeField, Range(0f, 1f)] private float damageMultOnMiss = 0.25f;
    [SerializeField, Range(0f, 1f)] private float speedMultOnMiss = 0.50f;
    [SerializeField] private float bulletAngle = 50;

    [Header("Special Buff Stats")]
    [SerializeField] private float statusDuration = 3;

    [Header("Normal Shot Stats")]
    [SerializeField] private float manaOnHit;
    [SerializeField] private float damage;
    [SerializeField] private float speed;

    [Header("Projectile Prefabs")]
    [SerializeField] private SaxofonProjectile normalProjectile;
    [SerializeField] private SaxofonSpecialProjectile specialProjectile;

    [Header("Additional Components")]
    [SerializeField] private SoundLibraryObject soundLibrary;

    protected override void Awake()
    {
        base.Awake();
        soundLibrary.Initialize();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void ExecuteBasicAttack()
    {
        base.ExecuteBasicAttack();

        ShootNormalProjectile(bulletAngle);
        ShootNormalProjectile(-bulletAngle);
    }

    public override void ExecuteSpecialAttack()
    {
        base.ExecuteSpecialAttack();
        ShootSpecialProjectile();
    }

    private void ShootNormalProjectile(float angle)
    {
        float dmgMultiplier = damageMultOnMiss;
        float speedMult = speedMultOnMiss;

        if (isOnBeat)
        {
            dmgMultiplier = beatCombo.currentRank.rankDamageMultiplier;
            speedMult = 1;
        }

        float finalDamage = damage * dmgMultiplier;
        float finalSpeed = speed * speedMult;

        SaxofonProjectile new1LvBullet = Instantiate(normalProjectile, transform.position, Quaternion.identity);
        new1LvBullet.InitializeProjectile(isOnBeat, manaComponent, manaOnHit, finalDamage, finalSpeed, new Vector2(transform.right.x, angle));

        string soundFxName = isOnBeat ? "OnBeatHit-" + 0.ToString() : "OnMissHit-" + 0.ToString();
        SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound(soundFxName)).Play();
    }

    private void ShootSpecialProjectile()
    {
        float dmgMultiplier = damageMultOnMiss;

        if (isOnBeat)
        {
            dmgMultiplier = beatCombo.currentRank.rankDamageMultiplier;
        }
        
        float finalDamage = damage * dmgMultiplier;

        SaxofonSpecialProjectile newSpecial = Instantiate(specialProjectile, transform.position, Quaternion.identity);

        newSpecial.SetDamage(finalDamage);

        if (isOnBeat)
        {
            SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnBeatHit-" + 2.ToString())).Play();
            Debug.Log("OnBeatHit-" + 2);
        }
        else
        {
            //projectile.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnMissHit-" + 2.ToString())).Play();
            Debug.Log("OnMissHit-" + 2);
        }

        newSpecial.GetComponent<SaxofonSpecialProjectile>().LaunchProjectile(transform.right);
    }
}

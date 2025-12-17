using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_SaxophoneV2 : PlayerWeapon
{
    [Header("Weapon Stats")]
    [SerializeField] private float damageOnBeatMiss = 1f;
    [SerializeField] private float damageOnBeat = 7f;
    [SerializeField] private float manaOnHit = 6f;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private Vector2 shootingAngle;

    [Header("Special Attack")]
    [SerializeField] private float specialBeatDamage;
    [SerializeField] private float specialMissDamage;

    [Header("Components")]
    [SerializeField] private SaxophoneBullet bulletPrefab;
    [SerializeField] private SaxophoneExplosion onBeatExplosion;
    [SerializeField] private SaxophoneExplosion onMissExplosion;

    [Header("Sounds")]
    [SerializeField] private SoundID OnBeatSound;
    [SerializeField] private SoundID OnMissSound;
    [SerializeField] private SoundID OnBeatSpecial;
    [SerializeField] private SoundID OnMissSpecial;

    protected override void Awake()
    {
        base.Awake();
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

        ShootProjectile();
    }

    public override void ExecuteSpecialAttack()
    {
        base.ExecuteSpecialAttack();
        
        SpawnExplosion();
    }

    private void ShootProjectile()
    {
        BroAudio.Play(isOnBeat ? OnBeatSound : OnMissSound);

        Vector2 direction = new Vector2(shootingAngle.x * transform.right.x, shootingAngle.y);

        SaxophoneBullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.InitializeProjectile(isOnBeat, manaComponent, manaOnHit, damageOnBeatMiss, damageOnBeat, bulletSpeed, direction, beatCombo);
    }

    private void SpawnExplosion()
    {
        SaxophoneExplosion newExplosion = Instantiate(isOnBeat? onBeatExplosion:onMissExplosion, transform.position, Quaternion.identity);
        newExplosion.InitializeExplosion(manaComponent, 0, isOnBeat ? specialBeatDamage:specialMissDamage, beatCombo);

        BroAudio.Play(isOnBeat? OnBeatSpecial:OnMissSpecial);
    }
}

using Ami.BroAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_Accordion : PlayerWeapon
{
    [Header("Weapon Stats")]
    [SerializeField, Range(0f, 1f)] private float damageMultOnMiss = 0.25f;
    [SerializeField, Range(0f, 1f)] private float speedMultOnMiss = 0.50f;

    [SerializeField] private int beatsFor2ndCharge;
    [SerializeField] private int beatsFor3rdCharge;
    [SerializeField, Range(0.7f, 1f)] private float chargeDurationMult = 1f;

    [Header("Special Buff Stats")]
    [SerializeField] private int buffBeatDuration;
    [SerializeField] private int buffBeatDurationOnMiss;

    [Header("1st Charge Shot Stats")]
    [SerializeField] private float manaOnHit_1st;
    [SerializeField] private float damage_1st;
    [SerializeField] private float speed_1st;

    [Header("2nd Charge Shot Stats")]
    [SerializeField] private float manaOnHit_2nd;
    [SerializeField] private float damage_2nd;
    [SerializeField] private float speed_2nd;

    [Header("3rd Charge Shot Stats")]
    [SerializeField] private float manaOnHit_3rd;
    [SerializeField] private float damage_3rd;
    [SerializeField] private float speed_3rd;

    [Header("Projectile Prefabs")]
    [SerializeField] private PlayerProjectile chargeLv1Prefab;
    [SerializeField] private PlayerProjectile chargeLv2Prefab;
    [SerializeField] private PlayerProjectile chargeLv3Prefab;

    [Header("Charge Flash")]
    [SerializeField] private Animator chargeFlashAnim;
    [SerializeField] private SpriteRenderer chargeFlashSprite;
    [SerializeField] private Color chargeLv1Color;
    [SerializeField] private Color chargeLv2Color;
    [SerializeField] private Color chargeLv3Color;
    [SerializeField] private Color specialModeColor;

    [Header("Sounds")]
    [SerializeField] private SoundID onBeatLv1;
    [SerializeField] private SoundID onBeatLv2;
    [SerializeField] private SoundID onBeatLv3;
    [SerializeField] private SoundID missBeatLv1;
    [SerializeField] private SoundID missBeatLv2;
    [SerializeField] private SoundID missBeatLv3;

    //Components
    private float currentCharge;
    private float timeFor2ndCharge;
    private float timeFor3rdCharge;

    private float specialBuffTimer;

    private float beatDuration;

    private bool isCharging;
    private bool isSpecialBuffActive;

    public static event Action OnspecialEnded = delegate { };

    protected override void Awake()
    {
        base.Awake();
        chargeFlashAnim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
    }

    private void Start()
    {
        beatDuration = 60f / BeatManager.Instance.BPM;

        timeFor2ndCharge = beatDuration * beatsFor2ndCharge * chargeDurationMult;
        timeFor3rdCharge = beatDuration * beatsFor3rdCharge * chargeDurationMult;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        BeatManager.Instance.intervals[0].OnBeatEvent += ChargeFlashOnBeat;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BeatManager.Instance.intervals[0].OnBeatEvent -= ChargeFlashOnBeat;
    }

    public override void ExecuteBasicAttack() 
    {
        base.ExecuteBasicAttack();

        if (currentCharge < 0.05f)
        {
            if (isSpecialBuffActive) Spawn3rdChargeBullet();
            else NonSpecialButtonPress();
            return;
        }

        if (isCharging && !isOnBeat)
        {
            Spawn1stChargeBullet();
            ResetCharge();
        }
        else if (currentCharge > timeFor3rdCharge)
        {
            Spawn3rdChargeBullet();
            ResetCharge();
        }
        else if (currentCharge > timeFor2ndCharge)
        {
            Spawn2ndChargeBullet();
            ResetCharge();
        }
    }

    private void NonSpecialButtonPress()
    {
        isCharging = true;
        anim.SetBool("IsCharging", true);
        chargeFlashAnim.SetBool("Active", true);
        Spawn1stChargeBullet();
    }

    public override void ExecuteSpecialAttack()
    {
        base.ExecuteSpecialAttack();
        isSpecialBuffActive = true;

        if (isOnBeat)
        {
            specialBuffTimer = buffBeatDuration * beatDuration;
            Spawn3rdChargeBullet();
        }
        else specialBuffTimer = buffBeatDurationOnMiss * beatDuration;
        chargeFlashAnim.SetBool("Active", true);
        chargeFlashSprite.color = specialModeColor;
    }

    protected override void Exit()
    {
        base.Exit();
        if (isSpecialBuffActive) anim.SetBool("Special", true);
    }

    private void Update()
    {
        int isButtonHeld = GameInputManager.Instance.PrimaryAttackInputStop;

        if (isButtonHeld == 1 && isCharging && currentCharge <= timeFor3rdCharge)
        {
            currentCharge += Time.deltaTime;
        }
        if (isButtonHeld == 0 && isCharging)
        {
            ResetCharge();
        }

        if (isCharging)
        {
            if (currentCharge >= timeFor3rdCharge)
            {
                chargeFlashSprite.color = chargeLv3Color;
            }
            else if (currentCharge >= timeFor2ndCharge)
            {
                chargeFlashSprite.color = chargeLv2Color;
            }
        }

        if (isSpecialBuffActive)
        {
            specialBuffTimer -= Time.deltaTime;

            if (specialBuffTimer <= 0)
            {
                isSpecialBuffActive = false;
                anim.SetBool("Special", false);
                chargeFlashAnim.SetBool("Active", false);
                chargeFlashSprite.color = chargeLv1Color;
                OnspecialEnded();
            }
        }
    }

    private void Spawn1stChargeBullet()
    {
        float dmgMultiplier = damageMultOnMiss;
        float speedMult = speedMultOnMiss;

        if (isOnBeat)
        {
            dmgMultiplier = beatCombo.GetDamageMultiplier();
            speedMult = 1;
        }

        float finalDamage = damage_1st * dmgMultiplier;
        float finalSpeed = speed_1st * speedMult;

        PlayerProjectile new1LvBullet = Instantiate(chargeLv1Prefab, transform.position, Quaternion.identity);
        new1LvBullet.InitializeProjectile(isOnBeat, manaComponent, manaOnHit_1st, finalDamage, finalSpeed, transform.right, beatCombo);

        BroAudio.Play(isOnBeat ? onBeatLv1 : missBeatLv1);
    }

    private void Spawn2ndChargeBullet()
    {
        float dmgMultiplier = damageMultOnMiss;
        float speedMult = speedMultOnMiss;

        if (isOnBeat)
        {
            dmgMultiplier = beatCombo.GetDamageMultiplier();
            speedMult = 1;
        }

        float finalDamage = damage_2nd * dmgMultiplier;
        float finalSpeed = speed_2nd * speedMult;


        PlayerProjectile new2LvBullet = Instantiate(chargeLv2Prefab, transform.position, Quaternion.identity);
        new2LvBullet.InitializeProjectile(isOnBeat, manaComponent, manaOnHit_2nd, finalDamage, finalSpeed, transform.right, beatCombo);

        BroAudio.Play(isOnBeat ? onBeatLv2 : missBeatLv2);
    }

    private void Spawn3rdChargeBullet() 
    {
        float dmgMultiplier = damageMultOnMiss;
        float speedMult = speedMultOnMiss;

        if (isOnBeat)
        {
            dmgMultiplier = beatCombo.GetDamageMultiplier();
            speedMult = 1;
        }

        float finalDamage = damage_3rd * dmgMultiplier;
        float finalSpeed = speed_3rd * speedMult;

        float finalManaOnHit = isSpecialBuffActive ? manaOnHit_1st : manaOnHit_3rd;

        PlayerProjectile new3LvBullet = Instantiate(chargeLv3Prefab, transform.position, Quaternion.identity);
        new3LvBullet.InitializeProjectile(isOnBeat, manaComponent, finalManaOnHit, finalDamage, finalSpeed, transform.right, beatCombo);

        BroAudio.Play(isOnBeat ? onBeatLv3 : missBeatLv3);
    }

    private void ResetCharge()
    {
        currentCharge = 0;
        isCharging = false;
        anim.SetBool("IsCharging", false);
        chargeFlashAnim.SetBool("Active", false);
        chargeFlashSprite.color = chargeLv1Color;
    }

    private void ChargeFlashOnBeat()
    {
        chargeFlashAnim.SetTrigger("OnBeat");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneWeapon : WeaponBasicFunctions
{
    private BoxCollider2D collider;

    private bool atk1 = false;
    private bool atk2 = false;

    [SerializeField] private float durartionTimer;
    private bool onBeat = false;

    private float currentDamage;
    private float currentMultiplier = 1;

    private bool onSpecialCooldown = false;
    private float specialCooldownTimer;

    void Start()
    {
        gameObject.GetComponent<BeatDetector>().OnBeat += ActivateBeatEffect;

        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        base.calculateCoolDown();
        if (onSpecialCooldown)
        {
            if (specialCooldownTimer < weaponInfo.SpecialCooldown)
            {
                specialCooldownTimer += Time.deltaTime;
            }
            else if (specialCooldownTimer >= weaponInfo.SpecialCooldown)
            {
                onSpecialCooldown = false;
                specialCooldownTimer = 0;
            }
        }
        if (durartionTimer >0)
        {
            durartionTimer -= Time.deltaTime;
        }
        else if (durartionTimer < 0)
        {
            durartionTimer = 0;
            //collider.enabled = false;
            ToggleCollider(false);
            atk1 = false;
            atk2 = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Atack1(weaponInfo.Damage, weaponInfo.Atk1Duration);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Atack2(weaponInfo.Damage, 3.2f, weaponInfo.Atk2Duration);
        }
    }

    public override void Atack1(float weaponAtk, float duration)
    {
        if (atk2 == false & onCooldown == false)
        {
            currentDamage = weaponAtk;
            currentMultiplier = 1;
            atk1 = true;
            durartionTimer = duration;
            //collider.enabled = true;
            ToggleCollider(true);
            onCooldown = true;
        }
    }

    public override void Atack2(float weaponAtk, float duration, float dmgMultiplier)
    {
        if (atk1 == false & onCooldown == false & onSpecialCooldown == false)
        {
            currentDamage = weaponAtk;
            currentMultiplier = dmgMultiplier;
            atk2 = true;

            //collider.enabled = true;
            ToggleCollider(true);

            durartionTimer = duration;
            if (!onBeat)
            {                
                onSpecialCooldown = true;
            }           
            
        }
    }

    private void ActivateBeatEffect(bool activate)
    {
        onBeat = activate;
    }

    private void ToggleCollider(bool active)
    {
        if (collider != null) 
        { 
            collider.enabled = active;
            gameObject.GetComponent<SpriteRenderer>().enabled = active;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent<EnemyBasicFunctions>(out EnemyBasicFunctions enemy);
        if (enemy != null)
        {
            enemy.TakeDamage(currentDamage * currentMultiplier);
        }        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneWeapon : WeaponBasicFunctions
{
    private BoxCollider2D weaponCollider;

    private bool atk1 = false;
    private bool atk2 = false;

    [SerializeField] private float durartionTimer;
    private bool onBeat = false;

    private float currentDamage;
    private float currentMultiplier = 1;
    private float beatDmgMultiplier;

    private bool onSpecialCooldown = false;
    private float specialCooldownTimer;

    void Start()
    {
        gameObject.GetComponent<BeatDetector>().OnBeat += ActivateBeatEffect;

        weaponCollider = this.GetComponent<BoxCollider2D>();
        weaponCollider.enabled = false;

        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        beatDmgMultiplier = weaponInfo.OnBeatDmgMultiplier;
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

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Atack1(weaponInfo.Damage, weaponInfo.Atk1Duration);
        //}
        //else if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    Atack2(weaponInfo.Damage, weaponInfo.Atk2Duration, weaponInfo.SpecialDmgMultiplier);
        //}
    }

    public override void Atack1(float weaponAtk, float duration)
    {
        if (atk2 == false & onCooldown == false)
        {
            currentDamage = weaponAtk;
            currentMultiplier = 1;
            if (onBeat) { currentDamage = currentDamage*beatDmgMultiplier; }
            atk1 = true;
            durartionTimer = duration;
            //collider.enabled = true;
            ToggleCollider(true);
            onCooldown = true;
        }
    }

    public override void Atack2(float weaponAtk, float duration, float dmgMultiplier = 1)
    {
        if (atk1 == false & onCooldown == false & onSpecialCooldown == false)
        {
            currentDamage = weaponAtk;
            currentMultiplier = dmgMultiplier;
            currentDamage = currentDamage*dmgMultiplier;
            if (onBeat) { currentDamage = currentDamage * beatDmgMultiplier; }
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
        if (weaponCollider != null) 
        {
            weaponCollider.enabled = active;
            gameObject.GetComponent<SpriteRenderer>().enabled = active;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //float damage = 0;
        collision.gameObject.TryGetComponent<EnemyBasicFunctions>(out EnemyBasicFunctions enemy);
        if (enemy != null)
        {
            //damage = currentDamage * currentMultiplier;            
            enemy.TakeDamage(currentDamage);
            Debug.Log("WepDamage: "+currentDamage /*+ ", Mult: "+currentMultiplier+", BtMult: "+beatDmgMultiplier*/);
        }
    }

}

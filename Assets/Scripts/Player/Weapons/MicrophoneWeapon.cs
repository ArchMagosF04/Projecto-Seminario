using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneWeapon : WeaponBasicFunctions
{
    private BoxCollider2D collider;

    private bool atk1 = false;
    private bool atk2 = false;

    private float durartionTimer;
    private bool onBeat = false;


    void Start()
    {
        gameObject.GetComponent<BeatDetector>().OnBeat += ActivateBeatEffect;

        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }

    void Update()
    {
        base.calculateCoolDown();
        if (durartionTimer !=0)
        {
            durartionTimer -= Time.deltaTime;
        }
        else if (durartionTimer < 0)
        {
            durartionTimer = 0;
            collider.enabled = false;
            atk1 = false;
            atk2 = false;
        }
    }

    public override void Atack1(float weaponAtk, float duration)
    {
        if (atk2 == false & onCooldown == false)
        {
            atk1 = true;
            durartionTimer = duration;
            collider.enabled = true;
            onCooldown = true;
        }
    }

    public override void Atack2(float weaponAtk, float duration, float dmgMultiplier)
    {
        if (atk1 == false & onCooldown == false)
        {
            atk2 = true;
            collider.enabled = true;
            if (!onBeat)
            {
                durartionTimer = duration;
                onCooldown = true;
            }           
            
        }
    }

    private void ActivateBeatEffect(bool activate)
    {
        onBeat = activate;
    }

    private void ToggleCollider()
    {
        if (collider != null) 
        { 
            collider.enabled = !collider.enabled;
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
        }
    }

}

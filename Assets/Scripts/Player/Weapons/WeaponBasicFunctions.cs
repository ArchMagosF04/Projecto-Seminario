using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBasicFunctions : MonoBehaviour
{
    [SerializeField] protected private WeaponStats weaponInfo;

    protected private bool onCooldown;
    protected private float coolDownTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
   protected void calculateCoolDown()
    {
        if (onCooldown) 
        {
            if (coolDownTimer < weaponInfo.AtkSpeed)
            {
                coolDownTimer += Time.deltaTime;
            }
            else if (coolDownTimer >= weaponInfo.AtkSpeed)
            {
                onCooldown = false;
                coolDownTimer = 0;
            }
        }
    }

    public abstract void Atack1(float weaponAtk, float duration);
    public abstract void Atack2(float weaponAtk, float duration, float dmgMultiplier);

}

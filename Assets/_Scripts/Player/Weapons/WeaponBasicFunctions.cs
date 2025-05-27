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

    public virtual void MakeAttack(int atkType)
    {
        switch (atkType) 
        {
            case 1: Atack1(weaponInfo.Damage, weaponInfo.Atk1Duration);
                break;

            case 2: Atack2(weaponInfo.Damage, weaponInfo.Atk2Duration, weaponInfo.SpecialDmgMultiplier);
                break;
        }
    }

    public float getCooldownTime()
    {
        return weaponInfo.AtkSpeed;
    }

    public bool isInCooldown()
    {
        return onCooldown;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_Component : MonoBehaviour
{
    protected PlayerWeapon weapon;

    protected SO_WeaponData data;

    protected WeaponAnimationEventHandler EventHandler => weapon.EventHandler;

    protected Core core => weapon.Core;

    protected bool isAttackActive;

    public virtual void Init(PlayerWeapon weapon, SO_WeaponData data)
    {
        this.weapon = weapon;
        this.data = data;
    }

    protected virtual void OnEnable()
    {
        weapon.OnEnter += HandleEnter;
        weapon.OnExit += HandleExit;
    }

    protected virtual void HandleEnter()
    {
        isAttackActive = true;
    }

    protected virtual void HandleExit()
    {
        isAttackActive = false;
    }

    protected virtual void OnDisable()
    {
        weapon.OnEnter -= HandleEnter;
        weapon.OnExit -= HandleExit;
    }
}

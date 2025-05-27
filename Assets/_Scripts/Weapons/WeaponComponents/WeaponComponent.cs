using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    protected PlayerWeapon weapon;
    protected WeaponAnimationEventHandler EventHandler => weapon.EventHandler; //If Null fix sequence issue.
    protected Core Core => weapon.Core;
    protected SO_WeaponData WeaponData => weapon.Data;

    protected virtual void Awake()
    {
        weapon = GetComponent<PlayerWeapon>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
}

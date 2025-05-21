using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic", order = 0)]
public class SO_WeaponData : ScriptableObject
{
    [field: SerializeField] public int NumberOfAttacks {  get; private set; }


    [Header("Weapon Movement")]
    [field: SerializeField] public AD_Movement[] MovementData { get; private set; }

    [Header("Weapon Damage")]
    [field: SerializeField] public AD_Damage[] DamageData { get; private set; }

    [Header("Weapon Knockback")]
    [field: SerializeField] public AD_Knockback[] KnockbackData { get; set; }

    [Header("Special Attack")]
    [field: SerializeField] public AD_Movement SpecialMovementData { get; private set; }
    [field: SerializeField] public AD_Damage SpecialDamageData { get; private set; }
    [field: SerializeField] public AD_Knockback SpecialKnockbackData { get; set; }
}

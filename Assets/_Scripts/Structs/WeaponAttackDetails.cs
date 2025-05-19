using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttackDetails
{
    [field: SerializeField] public string AttackName {  get; private set; }


    [Header("Movement")]
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public Vector2 Angle { get; private set; }


    [Header("Damage")]
    [field: SerializeField] public float DamageAmount { get; private set; }


    [Header("KnockBack")]
    [field: SerializeField] public float KnockbackStrength { get; private set; }
    [field: SerializeField] public Vector2 KnockbackAngle { get; private set; }
}

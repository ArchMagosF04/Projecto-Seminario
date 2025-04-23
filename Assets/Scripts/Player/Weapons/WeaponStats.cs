using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stats", menuName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    private float damage;
    public float Damage { get { return damage; } }
    private float atkSpeed;
    public float AtkSpeed { get { return atkSpeed; } }
    private float specialcharge;
    public float Specialcharge { get { return specialcharge; } }
}

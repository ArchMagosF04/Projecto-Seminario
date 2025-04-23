using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stats", menuName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    [SerializeField]private float damage;
    public float Damage { get { return damage; } }
    [SerializeField]private float atkSpeed;
    public float AtkSpeed { get { return atkSpeed; } }
    [SerializeField] private float specialcharge;
    public float Specialcharge { get { return specialcharge; } }

    [SerializeField] private float atk1Duration;
    public float Atk1Duration { get { return atk1Duration; } }
    [SerializeField] private float atk2Duration;
    public float Atk2Duration { get { return atk2Duration; } }

    [SerializeField] private float specialCooldown;
    public float SpecialCooldown { get { return specialCooldown; } }
}

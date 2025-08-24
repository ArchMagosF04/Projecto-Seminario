using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGardelData", menuName = "Data/Bosses Data/Gardel/Gardel Stats")]
public class GardelStats : ScriptableObject
{
    [field: Header("Idle State")]
    [field: SerializeField] public int BeatsSpentOnIdle { get; private set; } = 3;
    [field: SerializeField, Range(0, 1f)] public float StunAttackChance { get; private set; } = 0.1f;
    [field: SerializeField, Range(0, 1f)] public float StunAttackChanceAtHalf { get; private set; } = 0.35f;

    [field: Header("Jump State")]
    [field: SerializeField] public float JumpForce { get; private set; } = 25f;

    [field: Header("Normal Attack State")]
    [field: SerializeField] public int NumberOfAttacks { get; private set; } = 3;

    [field: Header("Special Attack State")]
    [field: SerializeField] public int SpecialBeatsToWait { get; private set; } = 1;

    [field: Header("Stun Attack State")]
    [field: SerializeField] public int StunBeatsToWait { get; private set; } = 1;
    [field: SerializeField] public int StunEffectBeatDuration { get; private set; } = 3;
    [field: SerializeField] public ScreenShakeProfile StunShakeProfile { get; private set; }

    [field: Header("Attack Prefabs")]
    [field: SerializeField] public Projectile[] Projectiles { get; private set; }
    [field: SerializeField] public GameObject ShoutAOEPrefab { get; private set; }
}

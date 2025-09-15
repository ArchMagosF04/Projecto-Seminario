using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAnittaData", menuName = "Data/Bosses Data/Anitta/Anitta Stats")]
public class AnittaStats : ScriptableObject
{
    [field: Header("Idle State")]
    [field: SerializeField] public int BeatsSpentOnIdle { get; private set; } = 3;
    [field: SerializeField, Range(0, 1f)] public float SpecialAttackChance { get; private set; } = 0.25f;
    [field: SerializeField, Range(0, 1f)] public float SecretAttackChance { get; private set; } = 0.1f;

    [field: Header("Jump State")]
    [field: SerializeField] public float JumpForce { get; private set; } = 25f;

    [field: Header("Teleport State")]
    [field: SerializeField] public int BeatsBeforeReappearance { get; private set; } = 2;

    [field: Header("Normal Attack State")]
    [field: SerializeField] public int BeatsBeforeNormalAttack { get; private set; } = 2;

    [field: Header("Special Attack State")]
    [field: SerializeField] public int SpecialBeatsToWait { get; private set; } = 1;

    [field: Header("Stun Attack State")]
    [field: SerializeField] public int StunBeatsToWait { get; private set; } = 1;
    [field: SerializeField] public int StunEffectBeatDuration { get; private set; } = 3;
    [field: SerializeField] public ScreenShakeProfile StunShakeProfile { get; private set; }

    //[field: Header("Attack Prefabs")]
    
}

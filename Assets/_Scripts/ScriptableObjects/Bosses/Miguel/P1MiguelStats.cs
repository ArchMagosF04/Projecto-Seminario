using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMiguelData", menuName = "Data/Bosses Data/Miguel/P1 Miguel Stats")]
public class P1MiguelStats : ScriptableObject
{
    [field: Header("Movement Stats")]
    [field: SerializeField] public float HorizontalSpeed { get; private set; } = 2f;
    [field: SerializeField] public float VerticalSpeed { get; private set; } = 1f;
    [field: SerializeField] public float OscillationAmplitude { get; private set; } = 1f;


    [field: Header("Idle State")]
    [field: SerializeField] public int BeatsSpentOnIdle { get; private set; } = 4;
    [field: SerializeField, Range(0, 1f)] public float NormalAttackChance { get; private set; } = 0.5f;
    [field: SerializeField, Range(0, 1f)] public float FlameAttackChance { get; private set; } = 0.5f;
    [field: SerializeField, Range(0, 1f)] public float SerpentAttackChance { get; private set; } = 0.33f;

    [field: Header("Normal Attack State")]
    [field: SerializeField] public int BeatsBeforeNormalAttack { get; private set; } = 2;

    [field: Header("Flame Attack State")]
    [field: SerializeField] public int BeatsBeforeFlameAttack { get; private set; } = 2;
}

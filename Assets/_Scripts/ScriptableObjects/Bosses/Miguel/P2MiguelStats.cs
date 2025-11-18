using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMiguelData", menuName = "Data/Bosses Data/Miguel/P2 Miguel Stats")]
public class P2MiguelStats : ScriptableObject
{
    [field: Header("Movement Stats")]
    [field: SerializeField] public float JumpForce {  get; private set; }


    [field: Header("Idle State")]
    [field: SerializeField] public int BeatsSpentOnIdle { get; private set; } = 3;
    [field: SerializeField, Range(0f, 1f)] public float NormalAttackChance { get; private set; } = 0.5f;
    [field: SerializeField, Range(0f, 1f)] public float SpecialAttackChance { get; private set; } = 0.4f;



    [field: Header("Normal Attack State")]
    [field: SerializeField] public int BeatsBeforeNormalAttack { get; private set; } = 2;

}

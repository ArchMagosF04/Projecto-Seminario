using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float hp;
    public float Hp {  get { return hp; } }
    [SerializeField] private float spd;
    public float Spd { get { return spd; } }
    [SerializeField] private float atk;
    public float Atk { get { return atk; } }

    [SerializeField] int specialAttackWeight;
    public int SpecialAttackWeight;

    [SerializeField] int maxTechniquePoints;
    public int MaxTechniquePoints;

}

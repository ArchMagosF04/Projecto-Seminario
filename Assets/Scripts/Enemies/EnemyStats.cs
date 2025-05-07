using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float hp;
    public float Hp {  get { return hp; } }
    [SerializeField] private float moveSpd;
    public float MoveSpd { get { return moveSpd; } }

    [SerializeField] private float gravity;
    public float Gravity { get { return gravity; } }

    [SerializeField] private float groundAcceleration;
    public float GroundAcceleration { get { return groundAcceleration; } }

    [SerializeField] private float groundDeceleration;
    public float GroundDeceleration { get { return groundDeceleration; } }

    [SerializeField] private float airAcceleration;
    public float AirAcceleration { get { return airAcceleration; } }

    [SerializeField] private float airDeceleration;
    public float AirDeceleration { get { return airDeceleration; } }

    [SerializeField] private float maxFallSpeed;
    public float MaxFallSpeed { get { return maxFallSpeed; } }

    [SerializeField] private float groundDetectionRayLength;
    public float GroundDetectionRayLength { get { return groundDetectionRayLength; } }

    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer { get { return groundLayer; } }

    [SerializeField] private float atk;
    public float Atk { get { return atk; } }

    [SerializeField] int specialAttackWeight;
    public int SpecialAttackWeight;

    [SerializeField] float specialAttackmultiplier;
    public float SpecialAttackmultiplier { get { return specialAttackmultiplier; } }

    [SerializeField] int maxTechniquePoints;
    public int MaxTechniquePoints;

    [SerializeField] private List<GameObject> projectiles;
    public GameObject GetProyectile(int proyectileIndex) {  return projectiles[proyectileIndex];}

}

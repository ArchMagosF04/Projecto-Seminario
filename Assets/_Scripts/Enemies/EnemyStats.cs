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

    // Jump Variables-----------------------------------------------------------------------

    [SerializeField] private float initialJumpVelocity;
    public float InitialJumpVelocity { get { return initialJumpVelocity; } }

    [SerializeField] private float apexThreshold;
    public float ApexThreshold { get { return apexThreshold; } }

    [SerializeField] private float apexHangTime;
    public float ApexHangTime { get { return apexHangTime; } }

    [SerializeField] private float gravityOnReleaseMultiplier;
    public float GravityOnReleaseMultiplier { get { return gravityOnReleaseMultiplier; } }

    [SerializeField] private float timeForUpwardsCancel;
    public float TimeForUpwardsCancel { get { return timeForUpwardsCancel; } }

    //-------------------------------------------------------------------------------------

    [SerializeField] private float atk;
    public float Atk { get { return atk; } }

    [SerializeField] private int specialAttackWeight;
    public int SpecialAttackWeight { get { return specialAttackWeight; } }

    [SerializeField] float specialAttackmultiplier;
    public float SpecialAttackmultiplier { get { return specialAttackmultiplier; } }

    [SerializeField] int maxTechniquePoints;
    public int MaxTechniquePoints;

    [SerializeField] private List<GameObject> projectiles;
    public GameObject GetProyectile(int proyectileIndex) {  return projectiles[proyectileIndex];}

}

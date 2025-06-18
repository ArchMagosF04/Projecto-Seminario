using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float hp;
    public float Hp {  get { return hp; } }

    //---------------MOVEMENT VARIABLES-------------------------------------------------------
    [SerializeField] private float moveSpd;
    public float MoveSpd { get { return moveSpd; } }

    [SerializeField] private float acceleration;
    public float Acceleration { get { return acceleration; } }   

    [SerializeField] private float groundDetectionRayLength;
    public float GroundDetectionRayLength { get { return groundDetectionRayLength; } }

    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer { get { return groundLayer; } }



    // Jump Variables-----------------------------------------------------------------------

    [SerializeField] private float jumpforce;
    public float JumpForce { get { return jumpforce; } }

    [SerializeField] private float maxFallSpeed;
    public float MaxFallSpeed { get { return maxFallSpeed; } }

    //-------------------------------------------------------------------------------------



    [SerializeField] private float atk;
    public float Atk { get { return atk; } }



    //--------------SPECIAL ATTACK VARIABLES----------------------------------------------

    [SerializeField] private int specialAttackWeight;
    public int SpecialAttackWeight { get { return specialAttackWeight; } }

    [SerializeField] float specialAttackmultiplier;
    public float SpecialAttackmultiplier { get { return specialAttackmultiplier; } }

    [SerializeField] private float specialAtkDuration;
    public float SpecialAtkDuration { get { return specialAtkDuration; } }

    [SerializeField] private float wavesSpawnInterval;
    public float WavesSpawnInterval { get { return wavesSpawnInterval; } }

    [SerializeField] private float wavesGrowthSpeed;
    public float WavesGrowthSpeed { get {return wavesGrowthSpeed; } }


    [SerializeField] private float wavesMaxSize;
    public float WavesMaxSize { get { return wavesMaxSize; } }


    [SerializeField] private float specialDelayTime = 3;
    public float SpecialDelayTime { get { return specialDelayTime; } }    

    //-------------------------------------------------------------------------------------



    [SerializeField] float timeToUseTechnique;
    public float TimeToUseTechnique { get { return timeToUseTechnique; } }

    [SerializeField] float techniqueDuration;
    public float TechniqueDuration { get { return techniqueDuration; } }

    [SerializeField] private float techniqueDelayTime = 3;
    public float TechniqueDelayTime { get { return techniqueDelayTime; } }



    [SerializeField] private List<GameObject> projectiles;
    public GameObject GetProyectile(int proyectileIndex) {  return projectiles[proyectileIndex];}

}

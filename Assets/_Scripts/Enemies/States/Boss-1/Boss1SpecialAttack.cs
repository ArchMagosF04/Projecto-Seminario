using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1SpecialAttack : State
{
    private BossMovement movementController;
    private Transform stageLocation;
    private GameObject boss;
    private GameObject atkProjectiles;
    private EnemyStats enemyInfo;
    private float dmg;
    private float specialDelayTime;
    private bool atCenterStage;
    private float currentSpecialDelayTime;
    private float specialAtkDuration;
    private float specialAtkTimer;
    private float wavesSpawnInterval;
    private float wavesIntervalTimer;
    private float wavesMaxSize;
    private float wavesGrowthSpeed;

    public Boss1SpecialAttack(BossMovement bossMovement, Transform targetLocation, GameObject user, float delayToStartAttack, float atkDuration, float atkIntervals, GameObject projectile, float projectileMaxSize, float projectileGrowthSpeed, EnemyStats enemyInformation)
    {
        movementController = bossMovement;
        stageLocation = targetLocation;
        boss = user;
        specialDelayTime = delayToStartAttack;
        specialAtkDuration = atkDuration;
        wavesSpawnInterval = atkIntervals;
        atkProjectiles = projectile;
        wavesMaxSize = projectileMaxSize;
        wavesGrowthSpeed = projectileGrowthSpeed;
        enemyInfo = enemyInformation;
    }   

    public override void FixedExecute()
    {
        //if (!atCenterStage)
        //{
        //    movementController.Move(enemyInfo.AirAcceleration, enemyInfo.AirDeceleration, new Vector2(FindCenterStage(), 0));
        //}
    }

    public override void Execute()
    {
        if (Mathf.Abs(boss.transform.position.x - stageLocation.position.x) < 0.1f)
        {
            if (currentSpecialDelayTime < specialDelayTime)
            {
                currentSpecialDelayTime += Time.deltaTime;
            }
            else
            {
                atCenterStage = true;
                currentSpecialDelayTime = 0;
            }
        }

        if (atCenterStage)
        {
            if (specialAtkTimer < specialAtkDuration)
            {
                if (wavesIntervalTimer <= 0)
                {
                    GameObject temp = GameObject.Instantiate(atkProjectiles, boss.transform.position, boss.transform.rotation/*quaternion.identity*/);
                    temp.gameObject.GetComponent<WaveAttack>().Initialize(enemyInfo.Atk * enemyInfo.SpecialAttackmultiplier, boss, wavesMaxSize, wavesGrowthSpeed);
                    wavesIntervalTimer = wavesSpawnInterval;
                }
                else
                {
                    wavesIntervalTimer -= Time.deltaTime;
                }
                specialAtkTimer += Time.deltaTime;
            }
            else
            {
                atCenterStage = false;
                specialAtkTimer = 0;
            }


        }
    }

    private float FindCenterStage()
    {
        Vector3 result = stageLocation.position - boss.transform.position;
        return result.normalized.x;
    }
}

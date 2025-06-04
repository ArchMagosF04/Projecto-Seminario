using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1SpecialAttack<T> : State<T>
{
    private BossMovement movementController;
    private Transform stageLocation;
    private GameObject boss;
    private GameObject atkProjectiles;
    private EnemyStats enemyInfo;
    private float specialDelayTime;
    private bool atCenterStage;
    public bool AtCenterStage {  get { return atCenterStage; } }

    private float currentSpecialDelayTime =0;
    private float wavesSpawnInterval;
    private float wavesIntervalTimer;
    private float wavesMaxSize;
    private float wavesGrowthSpeed;

    public Boss1SpecialAttack(BossMovement bossMovement, Transform targetLocation, GameObject user, EnemyStats enemyInformation)
    {
        movementController = bossMovement;
        stageLocation = targetLocation;
        boss = user;
        enemyInfo = enemyInformation;
        specialDelayTime = enemyInformation.SpecialDelayTime;
        wavesSpawnInterval = enemyInformation.WavesSpawnInterval;
        atkProjectiles = enemyInformation.GetProyectile(1);
        wavesMaxSize = enemyInformation.WavesMaxSize;
        wavesGrowthSpeed = enemyInformation.WavesGrowthSpeed;        
    }   

    public override void FixedExecute()
    {
        if (!atCenterStage)
        {
            movementController.Move(enemyInfo.Acceleration, new Vector2(FindCenterStage(), 0));
        }
    }

    public override void Execute()
    {
        if (Mathf.Abs(boss.transform.position.x - stageLocation.position.x) < 0.7f)
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
            if (wavesIntervalTimer <= 0)
            {
                GameObject temp = GameObject.Instantiate(atkProjectiles, boss.transform.position, boss.transform.rotation);
                temp.gameObject.GetComponent<WaveAttack>().Initialize(enemyInfo.Atk * enemyInfo.SpecialAttackmultiplier, boss, wavesMaxSize, wavesGrowthSpeed);
                wavesIntervalTimer = wavesSpawnInterval;
            }
            else
            {
                wavesIntervalTimer -= Time.deltaTime;
            }
        }
    }

    public override void Exit()
    {
        atCenterStage = false;
    }

    private float FindCenterStage()
    {
        Vector3 result = stageLocation.position - boss.transform.position;
        return result.normalized.x;
    }
}

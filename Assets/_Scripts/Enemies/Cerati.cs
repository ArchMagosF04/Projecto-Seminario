using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;

public class Cerati : EnemyBasicFunctions
{
    // Start is called before the first frame update
    private float time= 5;
    private float timer;
    private BossMovement movementComponent;
    [SerializeField] Transform centerStage;
    [SerializeField] Transform plataform1;
    [SerializeField] Transform plataform2;

    [SerializeField]private bool specialAttack;
    private bool atCenterStage;
    [SerializeField] private float specialAtkDuration;
    private float specialAtkTimer;
    [SerializeField] private GameObject guitarWaves;
    [SerializeField] private float wavesSpawnInterval;
    private float wavesIntervalTimer;
    [SerializeField] private float wavesGrowthSpeed;
    [SerializeField] private float wavesMaxSize;
    [SerializeField] private float specialDelayTime = 3;
    private float currentSpecialDelayTime = 0;
    [SerializeField] float waitToMakeChoices;
    private float currentTimeToMakeChoices = 0;
    private bool makeChoice = false;

    void Start()
    {
        movementComponent = gameObject.GetComponent<BossMovement>();
    }

    private void FixedUpdate()
    {
        if (specialAttack)
        {
            if (!atCenterStage)
            {
                movementComponent.Move(enemyInfo.AirAcceleration, enemyInfo.AirDeceleration, new Vector2(FindCenterStage(),0));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(currentTimeToMakeChoices < waitToMakeChoices)
        //{
        //    currentTimeToMakeChoices += Time.deltaTime;
        //}
        //else
        //{
        // makeChoice = true;   
        //}

        //if (makeChoice)
        //{
        //    int r = UnityEngine.Random.Range(1, 100);

        //    if (r < enemyInfo.SpecialAttackWeight)
        //    {
        //        specialAttack = true;
        //    }
        //    else
        //    {
        //        makeChoice = false;
        //        currentTimeToMakeChoices = 0;
        //    }
        //}



        if (!specialAttack)
        {
            if (timer < time)
            {
                timer += Time.deltaTime;
            }
            else if (timer > time)
            {
                timer = 0;
                BasicAttack();
            }
        }
        

        if (Mathf.Abs(transform.position.x - centerStage.position.x) < 0.1f && specialAttack)
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

        if(specialAttack && atCenterStage)
        {
            if(specialAtkTimer < specialAtkDuration)
            {
                if (wavesIntervalTimer <= 0)
                {
                    GameObject temp = GameObject.Instantiate(guitarWaves, transform.position, quaternion.identity);
                    temp.gameObject.GetComponent<WaveAttack>().Initialize(enemyInfo.Atk * enemyInfo.SpecialAttackmultiplier, this.gameObject, wavesMaxSize, wavesGrowthSpeed);
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
                specialAttack = false;
                atCenterStage = false;
                specialAtkTimer = 0;
            }

            
        }

        
    }

    protected override void BasicAttack()
    {
        GameObject bullet = GameObject.Instantiate(enemyInfo.GetProyectile(0), transform.position, Quaternion.Euler(0,0, GetPlayerRalativeDirection()));
        bullet.GetComponent<ProyectileBase>().Initialize(enemyInfo.Atk,this.gameObject);
    }



    protected override void SecretTechnique()
    {
        throw new System.NotImplementedException();
    }

    protected override void SpecialAttack()
    {
        specialAttack = true;
    }

    private float FindCenterStage()
    {
        Vector3 result = centerStage.position - transform.position;
        return result.normalized.x;
    }

    


}

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
    [SerializeField]private float specialAtkTimer;
    [SerializeField] private GameObject guitarWaves;
    [SerializeField] private float wavesSpawnInterval;
    private float wavesIntervalTimer;
    [SerializeField] private float wavesGrowthSpeed;
    [SerializeField] private float wavesMaxSize;
    [SerializeField] private float specialDelayTime = 3;
    private float currentSpecialDelayTime = 0;
    [SerializeField] float waitToMakeChoices = 1;
    private float currentTimeToMakeChoices = 0;
    private bool makeChoice = false;
    [SerializeField] float jumpforce;

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
                movementComponent.Move(7, new Vector2(FindCenterStage(), 0));
            }
        }

        //movementComponent.BossJumpUp(Vector2.up, jumpforce);
    }

    // Update is called once per frame
    void Update()
    {
        // Choose Atack
        if (currentTimeToMakeChoices < waitToMakeChoices)
        {
            currentTimeToMakeChoices += Time.deltaTime;
        }
        else
        {
            makeChoice = true;
        }

        if (makeChoice)
        {
            int r = UnityEngine.Random.Range(1, 100);

            print("rolled a: "+r);

            if (r < enemyInfo.SpecialAttackWeight)
            {
                specialAttack = true;
            }
            else
            {
                makeChoice = false;
                currentTimeToMakeChoices = 0;
            }
        }
        //--------------------------------------------------------



        if (!specialAttack)
        {
            if (movementComponent.IsGrounded)
            {
                float jumpDirection = Math.Sign(FindOpositePlataform().x - transform.position.x);
                RaycastHit2D plataformInFront = Physics2D.Raycast(transform.position, new Vector2(jumpDirection, 0), 3f, enemyInfo.GroundLayer);
            }

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


        // Position for special atk--------------------------------------------------------------------------
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
        //-----------------------------------------------------------------------------------------------------------

        //Special Atk---------------------------------------------------------------------------------------------------
        if (specialAttack && atCenterStage)
        {
            if (specialAtkTimer < specialAtkDuration)
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
        //---------------------------------------------------------------------------------------------------------------------------------








        //if (!bossCollider.enabled)
        //{
        //    if (currentTransparancyDuration < transparancyDuration)
        //    {
        //        currentTransparancyDuration += Time.deltaTime;
        //    }
        //    else if (currentTransparancyDuration >= transparancyDuration)
        //    {
        //        bossCollider.enabled = true;
        //        currentTransparancyDuration = 0;
        //    }
        //}

        //if (isGroundAbove)
        //{
        //    bossCollider.enabled = false;
        //}






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

    private Vector2 FindOpositePlataform()
    {
        if((plataform1.transform.position - transform.position).magnitude < (plataform2.transform.position - transform.position).magnitude)
        {
            return plataform2.position;
        }
        else
        {
            return plataform1.position;
        }
    }


}

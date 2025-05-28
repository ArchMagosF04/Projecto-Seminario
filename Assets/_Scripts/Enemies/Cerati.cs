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
    [SerializeField]private float timeToShoot= 5;
    private float timerToShoot;
    private BossMovement movementComponent;
    [SerializeField] Transform centerStage;
    [SerializeField] Transform plataform1;
    [SerializeField] Transform plataform2;

    [SerializeField]private bool specialAttack;
    [SerializeField] private bool technique;
    [SerializeField]private bool atCenterStage;
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
    //private bool makeChoice = false;
    [SerializeField] float jumpforce;
    private bool moving;
    private int shotsFired;
    private bool getBackUp;
    private GameObject targetPlataform;

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
                movementComponent.Move(8, new Vector2(FindCenterStage(), 0));
            }
        }

        if (shotsFired >= 3 && !getBackUp)
        {
            SwitchPlataform();
            shotsFired = 0;
        }

        if (getBackUp)
        {
            if (Mathf.Abs(targetPlataform.transform.position.x - transform.position.x) > 2f && movementComponent.IsGrounded)
            {
                movementComponent.Move(8f, (targetPlataform.transform.position - transform.position).normalized);
            }
            if (Mathf.Abs(targetPlataform.transform.position.x - transform.position.x) < 2f)
            {
                if(Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x) > 0)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
                }
                
                movementComponent.BossJumpUp(Vector2.up, jumpforce * 1.5f);
            }
        }

        //movementComponent.BossJumpUp(Vector2.up, jumpforce);
    }

    // Update is called once per frame
    void Update()
    {
        if (!specialAttack && movementComponent.IsGrounded && !getBackUp)
        {
            if (movementComponent.IsGrounded)
            {
                float jumpDirection = Math.Sign(FindOpositePlataform().position.x - transform.position.x);
                RaycastHit2D plataformInFront = Physics2D.Raycast(transform.position, new Vector2(jumpDirection, 0), 3f, enemyInfo.GroundLayer);
            }

            // Choose Atack
            if (currentTimeToMakeChoices < waitToMakeChoices)
            {
                currentTimeToMakeChoices += Time.deltaTime;
            }
            else
            {
                RollAtk();
            }


            //--------------------------------------------------------


            if (timerToShoot < timeToShoot)
            {
                timerToShoot += Time.deltaTime;
            }
            else if (timerToShoot > timeToShoot)
            {
                timerToShoot = 0;
                BasicAttack();
            }
        }

        if (!specialAttack && transform.position.y < plataform1.transform.position.y && !getBackUp)
        {
            getBackUp = true;

            int r = UnityEngine.Random.Range(1, 3);
            if (r > 1)
            {
                targetPlataform = plataform2.gameObject;
            }
            else
            {
                targetPlataform = plataform1.gameObject;
            }
        }
        else if (transform.position.y > plataform1.transform.position.y)
        {
            getBackUp = false;
        }


        // Position for special atk--------------------------------------------------------------------------
        if (Mathf.Abs(transform.position.x - centerStage.position.x) < 0.5f && specialAttack)
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















    }

    protected override void BasicAttack()
    {
        GameObject bullet = GameObject.Instantiate(enemyInfo.GetProyectile(0), transform.position, Quaternion.Euler(0,0, GetPlayerRalativeDirection()));
        bullet.GetComponent<ProyectileBase>().Initialize(enemyInfo.Atk,this.gameObject);
        shotsFired++;
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

    private Transform FindOpositePlataform()
    {
        if((plataform1.transform.position - transform.position).magnitude < (plataform2.transform.position - transform.position).magnitude)
        {
            return plataform2.transform;
        }
        else
        {
            return plataform1.transform;
        }
    }

    private void RollAtk()
    {
        int r = UnityEngine.Random.Range(1, 100);

        print("rolled a: " + r);

        if (r < enemyInfo.SpecialAttackWeight)
        {
            specialAttack = true;
        }
        else
        {
            currentTimeToMakeChoices = 0;
        }
    }

    private void SwitchPlataform()
    {
        Vector2 target = FindOpositePlataform().position;
        moving = true;
        float impulse = (FindOpositePlataform().position - transform.position).magnitude;
        movementComponent.BossJump(target, jumpforce + impulse/1.65f);
        print("impulse was: "+impulse);
    }   


}

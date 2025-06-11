using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss1NormalAttack<T> : State<T>
{
    private GameObject player;
    private GameObject plataform1;
    private GameObject plataform2;
    private GameObject targetPlataform;
    private BossMovement movementComponent;
    private EnemyStats enemyInfo;
    private GameObject boss;
    private int shotsFired = 0;
    private bool getBackUp = false;
    private bool moving;
    private BeatDetector shootBeatDetector;
    private BeatDetector jumpBeatdetector;

    private float timeToShoot = 1;
    private float timerToShoot;
    private bool beatEffect = false;
    private bool beatEffect2 = false;
    private bool shotFired = false;
    public Boss1NormalAttack(GameObject player, GameObject plataform1, GameObject plataform2, BossMovement movementComponent, GameObject user, EnemyStats enemyInfo, BeatDetector shootbeatDetector, BeatDetector jumpBeatDetector = null)
    {
        this.player = player;
        this.plataform1 = plataform1;
        this.plataform2 = plataform2;
        this.movementComponent = movementComponent;
        boss = user;
        this.enemyInfo = enemyInfo;
        this.shootBeatDetector = shootbeatDetector;

        if (jumpBeatDetector != null)
        {
            this.jumpBeatdetector = jumpBeatDetector;
        }
        else
        {
            jumpBeatdetector = shootBeatDetector;
        }
    }

    public override void Enter()
    {
        shootBeatDetector.OnBeat += ActivateBeatEffect1;
        jumpBeatdetector.OnBeat += ActivateBeatEffect2;
    }

    public override void FixedExecute()
    {
        if (shotsFired >= 3 && !getBackUp && movementComponent.IsGrounded && beatEffect2)
        {
            SwitchPlataform();
            shotsFired = 0;
        }

        if (getBackUp)
        {
            if (Mathf.Abs(targetPlataform.transform.position.x - boss.transform.position.x) > 2f && movementComponent.IsGrounded)
            {
                movementComponent.Move(8f, (targetPlataform.transform.position - boss.transform.position).normalized);
            }
            if (Mathf.Abs(targetPlataform.transform.position.x - boss.transform.position.x) < 2f)
            {
                if (Mathf.Abs(boss.GetComponent<Rigidbody2D>().velocity.x) > 0)
                {
                    boss.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
                }

                movementComponent.BossJumpUp(Vector2.up, enemyInfo.JumpForce * 1.34f);
            }
        }
    }

    public override void Execute()
    {
        if (movementComponent.IsGrounded && !getBackUp)
        {
            // Jump Between Plaraforms---------------------------------------------------------------------------------------------------------------
            //if (movementComponent.IsGrounded)
            //{
            //    float jumpDirection = Math.Sign(FindOpositePlataform().position.x - transform.position.x);
            //    //RaycastHit2D plataformInFront = Physics2D.Raycast(transform.position, new Vector2(jumpDirection, 0), 3f, enemyInfo.GroundLayer);
            //}

            // Choose Atack------------------------------------------------

            //--------------------------------------------------------

            //--Basic Attack----------------------------------------
            if(beatEffect && !shotFired)
            {
                BasicAttack();
                shotFired = true;
            }
            else if (shotFired && timerToShoot < timeToShoot)
            {
                timerToShoot += Time.deltaTime;
            }
            else if (timerToShoot > timeToShoot)
            {
                timerToShoot = 0;
                shotFired = false;
            }


            //--------------------------------------------------------
        }


        if (boss.transform.position.y < plataform1.transform.position.y && !getBackUp)
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
        else if (boss.transform.position.y > plataform1.transform.position.y)
        {
            getBackUp = false;
        }
    }

    public override void Exit()
    {
        shotsFired = 0;
        shotFired = false;
        shootBeatDetector.OnBeat -= ActivateBeatEffect1;
    }

    private void BasicAttack()
    {
        GameObject bullet = GameObject.Instantiate(enemyInfo.GetProyectile(0), boss.transform.position, Quaternion.Euler(0, 0, GetPlayerRalativeDirection()));
        bullet.GetComponent<ProyectileBase>().Initialize(enemyInfo.Atk, boss.gameObject, 7);
        shotsFired++;
    }
    


    private Transform FindOpositePlataform()
    {
        if ((plataform1.transform.position - boss.transform.position).magnitude < (plataform2.transform.position - boss.transform.position).magnitude)
        {
            return plataform2.transform;
        }
        else
        {
            return plataform1.transform;
        }
    }   

    private void SwitchPlataform()
    {
        Vector2 target = FindOpositePlataform().position;
        //moving = true;
        float impulse = (FindOpositePlataform().position - boss.transform.position).magnitude;
        movementComponent.Move(impulse / 1.1f, target);
        movementComponent.BossJump(target, enemyInfo.JumpForce + impulse / 1.60f);        
            
        Console.WriteLine("impulse was: " + impulse);
    }

    private protected float GetPlayerRalativeDirection()
    {
        Vector3 direction = player.transform.position - boss.transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void ActivateBeatEffect1(bool activation)
    {
        beatEffect = activation;
    }

    private void ActivateBeatEffect2(bool activation)
    {
        beatEffect2 = activation;
    }
}

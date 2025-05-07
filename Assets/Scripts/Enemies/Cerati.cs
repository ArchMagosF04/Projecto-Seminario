using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.Mathematics;
using UnityEngine;

public class Cerati : EnemyBasicFunctions
{
    // Start is called before the first frame update
    private float time= 5;
    private float timer;
    private BossMovement movementComponent;
    [SerializeField] Transform centerStage;
    [SerializeField]private bool specialAttack;
    private bool atCenterStage;
    [SerializeField] private float specialAtkDuration;
    private float specialAtkTimer;
    [SerializeField] private GameObject guitarWaves;
    [SerializeField] private float wavesSpawnInterval;
    private float wavesIntervalTimer;
    [SerializeField] private float wavesGrowthSpeed;
    [SerializeField] private float wavesMaxSize;

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
        

        if (transform.position.x == centerStage.position.x)
        {
            atCenterStage = true;
        }

        if(specialAttack && atCenterStage)
        {
            if(wavesIntervalTimer <= 0)
            {
                GameObject temp = GameObject.Instantiate(guitarWaves, transform.position, quaternion.identity);
                temp.gameObject.GetComponent<WaveAttack>().Initialize(atk * enemyInfo.SpecialAttackmultiplier, this.gameObject, wavesMaxSize, wavesGrowthSpeed);
                wavesIntervalTimer = wavesSpawnInterval;
            }
            else
            {
                wavesIntervalTimer -= Time.deltaTime;
            }

            
        }

        
    }

    protected override void BasicAttack()
    {
        GameObject bullet = GameObject.Instantiate(enemyInfo.GetProyectile(0), transform.position, Quaternion.Euler(0,0, GetPlayerRalativeDirection()));
        bullet.GetComponent<ProyectileBase>().Initialize(atk,this.gameObject);
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

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    [SerializeField] BossMovement movementController;
    [SerializeField] Transform centerStage;


    GenericFSM<string> fsm;
    Boss1SpecialAttack<string> specialAttackState;
    IState<string> normalAttackState;

    [SerializeField] private GameObject plataform1;
    [SerializeField] private GameObject plataform2;

    [SerializeField] private BeatDetector[] beatDetectors;


    //--------------SPECIAL ATTACK VARIABLES----------------------------------------------------------------------------------------

    [SerializeField] private bool doingSpecialAttack = false;    
    private float specialAtkTimer;
    //------------------------------------------------------------------------------------------------------------------------------

    //----Roll Variables--------------------------------------------------------------------------
    [SerializeField] bool RollAfterCertainAmountOfShots;
    [SerializeField] int amountOfShots;
    [SerializeField] float timeForTheNextRoll;
    private float currentTimeToMakeChoices = 0;
    private bool makeChoice = false;
    private bool rolledSpecialAttack = false;
    private bool rolledBasicAttack = false;
    private bool rolledTechnique = false;
    //---------------------------------------------------------------------------------------------

    [SerializeField] private protected EnemyStats enemyInfo;
    private protected GameObject player;
    [SerializeField]private float hp;
    private protected float spd;
    private protected float atk;
    private protected int techniquePoints;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        gameObject.GetComponent<HealthComponent>().AssignHealth(enemyInfo.Hp);
        spd = enemyInfo.MoveSpd;
        atk = enemyInfo.Atk;

        specialAttackState = new Boss1SpecialAttack<string>(movementController, centerStage, this.gameObject, enemyInfo, beatDetectors[0]);

        InitializeFSM();

    }

    private void FixedUpdate()
    {      

        fsm.OnFixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnUpdate();        

        if (specialAttackState.AtCenterStage && specialAtkTimer < enemyInfo.SpecialAtkDuration)
        {            
            specialAtkTimer += Time.deltaTime;
        }
        else if (specialAttackState.AtCenterStage && specialAtkTimer >= enemyInfo.SpecialAtkDuration)
        {
            fsm.ChangeState("normalAttack");
            specialAtkTimer = 0;            
        }

        if (currentTimeToMakeChoices < timeForTheNextRoll)
        {
            currentTimeToMakeChoices += Time.deltaTime;
        }
        else
        {
            RollAtk();
        }
    }





    private void InitializeFSM()
    {
        var _idle = new Boss1IdleState<string>();
        //var _specialAttack = new Boss1SpecialAttack<string>(movementController, centerStage, this.gameObject, enemyInfo);
        var _normalAttack = new Boss1NormalAttack<string>(player,plataform1, plataform2, movementController, this.gameObject, enemyInfo, beatDetectors[0], beatDetectors[2]);

        fsm = new GenericFSM<string>(_normalAttack);
        fsm.SetInitialState(_normalAttack);

        _normalAttack.AddTransition("specialAttack", specialAttackState);
        specialAttackState.AddTransition("normalAttack", _normalAttack);
    }

    private void RollAtk()
    {
        int r = UnityEngine.Random.Range(1, 100);

        Console.WriteLine("rolled a: " + r);

        if (r < enemyInfo.SpecialAttackWeight)
        {
            fsm.ChangeState("specialAttack");
        }
        else
        {
            currentTimeToMakeChoices = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }        
    }

}

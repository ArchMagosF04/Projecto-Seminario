using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    [SerializeField] BossMovement movementController;
    [SerializeField] Transform centerStage;
    [SerializeField] GameObject cameraToShake;

    [field: SerializeField] public Animator Anim { get; private set; }

    GenericFSM<string> fsm;
    Boss1SpecialAttack<string> specialAttackState;
    Boss1Technique<string> techniqueState;
    IState<string> normalAttackState;

    [SerializeField] private GameObject plataform1;
    [SerializeField] private GameObject plataform2;

    [SerializeField] private BeatDetector[] beatDetectors;

    [SerializeField] private float knockbackForce;

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

    public bool doingTechnique;
    [SerializeField]private protected float techniqueTime;
    private float techniqueCurrentDuration;
    [SerializeField] GameObject playerAnchor;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        gameObject.GetComponent<HealthComponent>().AssignHealth(enemyInfo.Hp);
        spd = enemyInfo.MoveSpd;
        atk = enemyInfo.Atk;

        specialAttackState = new Boss1SpecialAttack<string>(movementController, centerStage, this.gameObject, enemyInfo, beatDetectors[0], Anim);
        techniqueState = new Boss1Technique<string>(movementController, centerStage, this.gameObject, plataform1, plataform2, enemyInfo, beatDetectors[3], cameraToShake, player, playerAnchor, Anim);

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

        if (!doingTechnique)
        {
            techniqueTime += Time.deltaTime;
        }

        if (specialAttackState.AtCenterStage && specialAtkTimer < enemyInfo.SpecialAtkDuration)
        {            
            specialAtkTimer += Time.deltaTime;
        }
        else if (specialAttackState.AtCenterStage && specialAtkTimer >= enemyInfo.SpecialAtkDuration)
        {
            fsm.ChangeState("normalAttack");
            specialAtkTimer = 0;            
        }

        if (techniqueState.AtCenterStage && techniqueCurrentDuration < enemyInfo.TechniqueDuration)
        {
            techniqueCurrentDuration += Time.deltaTime;
        }
        else if (techniqueState.AtCenterStage && techniqueCurrentDuration >= enemyInfo.TechniqueDuration)
        {
            techniqueTime = 0;
            techniqueCurrentDuration = 0;
            doingTechnique = false;
            RollAtk();            
        }

        if (currentTimeToMakeChoices < timeForTheNextRoll && !doingTechnique)
        {
            currentTimeToMakeChoices += Time.deltaTime;
        }
        else if (currentTimeToMakeChoices >= timeForTheNextRoll && !doingTechnique)
        {
            RollAtk();
        }
    }





    private void InitializeFSM()
    {
        var _idle = new Boss1IdleState<string>();

        //var _specialAttack = new Boss1SpecialAttack<string>(movementController, centerStage, this.gameObject, enemyInfo);

        var _normalAttack = new Boss1NormalAttack<string>(player, plataform1, plataform2, movementController, this.gameObject, enemyInfo, beatDetectors[0], Anim, beatDetectors[2]);

        fsm = new GenericFSM<string>(_normalAttack);
        fsm.SetInitialState(_normalAttack);

        _normalAttack.AddTransition("specialAttack", specialAttackState);
        _normalAttack.AddTransition("techniqueAttack", techniqueState);

        specialAttackState.AddTransition("normalAttack", _normalAttack);
        specialAttackState.AddTransition("techniqueAttack", techniqueState);

        techniqueState.AddTransition("specialAttack", specialAttackState);
        techniqueState.AddTransition("normalAttack", _normalAttack);
    }

    private void RollAtk()
    {
        if(techniqueTime >= enemyInfo.TimeToUseTechnique)
        {
            fsm.ChangeState("techniqueAttack");
            doingTechnique = true;
        }
        else
        {
            int r = UnityEngine.Random.Range(1, 100);

            Debug.Log("rolled a: " + r);

            if (r < enemyInfo.SpecialAttackWeight)
            {
                fsm.ChangeState("specialAttack");
                currentTimeToMakeChoices = 0;
            }
            else
            {
                fsm.ChangeState("normalAttack");
                currentTimeToMakeChoices = 0;
            }
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthComponent>().TakeDamage(enemyInfo.Atk);
            collision.gameObject.GetComponentInChildren<KnockBackReceiver>().KnockBack((player.transform.position - transform.position).normalized, knockbackForce, 5);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * knockbackForce*10, ForceMode2D.Impulse);
        }        
    }

}

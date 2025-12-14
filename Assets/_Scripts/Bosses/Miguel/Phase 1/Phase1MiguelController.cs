using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Phase1MiguelController : MonoBehaviour
{
    #region State Machine

    public StateMachine StateMachine { get; private set; }
    public P1MiguelST_Idle IdleState { get; private set; }
    public P1MiguelST_FlameAttack FlameAttack { get; private set; }

    public P1MiguelST_NormalAttack NormalAttack { get; private set; }

    #endregion

    #region Components

    public Core Core { get; private set; }

    private Core_Movement movement;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private CinemachineImpulseSource impulseSource;
    public BeamWeapon beamWeapon {  get; private set; }

    [Header("Scriptable Objects")]
    [SerializeField] private P1MiguelStats miguelStats;

    [Header("Waypoints")]
    [SerializeField] private Transform rightWaypoint;
    [SerializeField] private Transform leftWaypoint;
    private bool goingLeft = true;
    private bool goingUp = true;

    [Header("Platforms")]
    [SerializeField] public Lv3FloorsManager FloorsManager;

    #endregion

    #region Other Variables

    public enum ActionType { None, Normal, Special }
    public ActionType DesiredAction = ActionType.None;

    private Transform player;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        movement = Core.GetCoreComponent<Core_Movement>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);

        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();
        beamWeapon = GetComponentInChildren<BeamWeapon>();

        StateMachine = new StateMachine();
        IdleState = new P1MiguelST_Idle(this, StateMachine, miguelStats, anim, "Idle");
        FlameAttack = new P1MiguelST_FlameAttack(this, StateMachine, miguelStats, anim, "Attack");
        NormalAttack = new P1MiguelST_NormalAttack(this, StateMachine, miguelStats, anim, "Attack");
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        player = GameManager.Instance.PlayerInstance.transform;
    }

    private void OnEnable()
    {
        animatorEvent.OnAnimationFinishedTrigger += AnimationFinishedTrigger;
    }

    private void OnDisable()
    {
        IdleState.UnsubscribeToEvents();
        FlameAttack.UnsubscribeToEvents();
        NormalAttack.UnsubscribeToEvents();

        animatorEvent.OnAnimationFinishedTrigger -= AnimationFinishedTrigger;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        StateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive) return;

        StateMachine.CurrentState.OnFixedUpdate();
        
        CheckFlip(player);

        HorizontalMovement();
        VerticalMovement();
    }

    #endregion

    public void HorizontalMovement()
    {   
        if (goingLeft)
        {
            if (transform.position.x <= leftWaypoint.position.x)
            {
                goingLeft = false;
                return;
            }

            movement.SetVelocityX(-miguelStats.HorizontalSpeed);
        }
        else
        {
            if (transform.position.x >= rightWaypoint.position.x)
            {
                goingLeft = true;
                return;
            }

            movement.SetVelocityX(miguelStats.HorizontalSpeed);
        }
    }

    public void VerticalMovement()
    {
        if (goingUp)
        {
            if (transform.position.y >= leftWaypoint.position.y + miguelStats.OscillationAmplitude)
            {
                goingUp = false;
                return;
            }

            movement.SetVelocityY(miguelStats.VerticalSpeed);
        }
        else
        {
            if (transform.position.y <= rightWaypoint.position.y - miguelStats.OscillationAmplitude)
            {
                goingUp = true;
                return;
            }

            movement.SetVelocityY(-miguelStats.VerticalSpeed);
        }
    }

    #region Other Functions

    public void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    public void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }

    public void CheckFlip(Transform target)
    {
        int direction = 0;

        if (target.position.x > transform.position.x) direction = 1;
        else direction = -1;

        movement.FlipCheck(direction);
    }

    public float GetHealth()
    {
        return 100;
    }

    #endregion
}

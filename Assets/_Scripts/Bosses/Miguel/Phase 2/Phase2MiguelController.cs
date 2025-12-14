using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2MiguelController : MonoBehaviour
{
    #region State Machine

    public StateMachine StateMachine { get; private set; }
    public P2MiguelST_Idle IdleState { get; private set; }
    public P2MiguelST_Jump JumpState { get; private set; }
    public P2MiguelST_Airborne AirborneState { get; private set; }
    public P2MiguelST_NormalAttack NormalAttack { get; private set; }
    public P2MiguelST_SpecialAttack SpecialAttack { get; private set; }

    #endregion

    #region Components

    public Core Core { get; private set; }

    private Core_Movement movement;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private CinemachineImpulseSource impulseSource;

    [Header("Scriptable Objects")]
    [SerializeField] private P2MiguelStats miguelStats;

    [Header("Attack References")]
    public BeamWeapon[] skyBeams;
    public BeamWeapon[] bodyBeams;
    [field: SerializeField] public SerpentController serpentController {  get; private set; }

    [field: Header("Waypoints")]
    [field: SerializeField] public Transform RightWaypoint;
    [field: SerializeField] public Transform LeftWaypoint;

    #endregion

    #region Other Variables

    public enum ActionType { None, Normal, Jump, Special }
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

        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();

        StateMachine = new StateMachine();
        IdleState = new P2MiguelST_Idle(this, StateMachine, miguelStats, anim, "Idle");
        JumpState = new P2MiguelST_Jump(this, StateMachine, miguelStats, anim, "Jump");
        NormalAttack = new P2MiguelST_NormalAttack(this, StateMachine, miguelStats, anim, "Attack");
        AirborneState = new P2MiguelST_Airborne(this, StateMachine, miguelStats, anim, "InAir");
        SpecialAttack = new P2MiguelST_SpecialAttack(this, StateMachine, miguelStats, anim, "Attack");
    }

    private void Start()
    {
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
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
        JumpState.UnsubscribeToEvents();
        NormalAttack.UnsubscribeToEvents();
        AirborneState.UnsubscribeToEvents();
        SpecialAttack.UnsubscribeToEvents();

        animatorEvent.OnAnimationFinishedTrigger -= AnimationFinishedTrigger;
    }

    private void Update()
    {
        StateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.OnFixedUpdate();
    }

    #endregion

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
       return Core.GetCoreComponent<Core_Health>().CurrentHealth;
    }

    #endregion
}

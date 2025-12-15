using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaController : MonoBehaviour
{
    #region State Machine Varibles
    public StateMachine StateMachine { get; private set; }
    public AnittaST_Idle IdleState { get; private set; }
    public AnittaST_Teleport TeleportState { get; private set; }
    public AnittaST_NormalAttack NormalAttackState { get; private set; }
    public AnittaST_SpecialAttack SpecialAttackState { get; private set; }
    #endregion

    #region Component References
    public Core Core { get; private set; }

    private Core_Health health;
    private Core_Movement movement;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private CinemachineImpulseSource impulseSource;

    [Header("Teleport Components")]
    public Collider2D DamageCollider;
    public SpriteRenderer TargetIndicator;

    [Header("Scriptable Objects")]
    [SerializeField] private AnittaStats anittaStats;

    [field: Header("Boss Waypoints")]
    [field: SerializeField] public Transform[] PlatformsTransforms {  get; private set; }
    [field: SerializeField] public PushPlatform[] PushPlatforms { get; private set; }
    [field: SerializeField] private GameObject Cars;

    #endregion

    #region Other Variables
    [Header("Status")]
    public bool LastAttackWasSpecial;
    public Transform DesiredJumpTarget;
    public Transform LastJumpTarget;

    public enum ActionType { None, Normal, Special }
    public ActionType DesiredAction = ActionType.None;

    #endregion

    #region Unity Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        health = Core.GetCoreComponent<Core_Health>();
        movement = Core.GetCoreComponent<Core_Movement>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);

        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();

        StateMachine = new StateMachine();
        IdleState = new AnittaST_Idle(this, StateMachine, anittaStats, anim, "Idle");
        TeleportState = new AnittaST_Teleport(this, StateMachine, anittaStats, anim, "Teleport");
        NormalAttackState = new AnittaST_NormalAttack(this, StateMachine, anittaStats, anim, "NormalAttack");
        SpecialAttackState = new AnittaST_SpecialAttack(this, StateMachine, anittaStats, anim, "SpecialAttack");
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        TargetIndicator.enabled = false;
        LastJumpTarget = PlatformsTransforms[0];
    }

    private void OnEnable()
    {
        animatorEvent.OnAnimationFinishedTrigger += AnimationFinishedTrigger;
    }

    private void OnDisable()
    {
        IdleState.UnsubscribeToEvents();
        TeleportState.UnsubscribeToEvents();
        NormalAttackState.UnsubscribeToEvents();
        SpecialAttackState.UnsubscribeToEvents();

        animatorEvent.OnAnimationFinishedTrigger -= AnimationFinishedTrigger;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        StateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive)
        {
            movement.SetVelocityX(0);
            return;
        }

        StateMachine.CurrentState.OnFixedUpdate();
    }

    #endregion

    #region Boss Attacks

    public void FireProjectile()
    {
        GameObject newNote = Instantiate(anittaStats.SeekingProjectile, transform.position + new Vector3(0, 1.5f), Quaternion.identity);
    }

    public void FireTwinProjectiles()
    {
        GameObject newNote = Instantiate(anittaStats.SeekingProjectile, transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        GameObject newNote2 = Instantiate(anittaStats.SeekingProjectile2, transform.position - new Vector3(0, 1.5f), Quaternion.identity);
    }

    public void FireWave()
    {
        if (IsAtSecondPhase())
        {
            GameObject newNote = Instantiate(anittaStats.WaveProjectile2, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject newNote2 = Instantiate(anittaStats.WaveProjectile, transform.position, Quaternion.identity);
        }
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

    public bool IsAtSecondPhase()
    {
        if (health.CurrentHealth / health.MaxHealth <= 0.5) return true;
        return false;
    }

    public void SecondPhaseAdditions()
    {
        Cars.SetActive(true);

        foreach (PushPlatform platform in PushPlatforms)
        {
            platform.EnterSecondPhase();
        }
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
        return health.CurrentHealth;
    }

    #endregion
}

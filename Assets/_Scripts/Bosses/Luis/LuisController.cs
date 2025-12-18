using Ami.BroAudio;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LuisController : MonoBehaviour
{
    #region State Machine

    public StateMachine StateMachine { get; private set; }
    public LuisST_Idle IdleState { get; private set; }
    public LuisST_Jump JumpState { get; private set; }
    public LuisST_Airborne AirborneState { get; private set; }
    public LuisST_NormalAttack NormalAttack { get; private set; }
    public LuisST_SpecialAttack SpecialAttack { get; private set; }
    public LuisST_Death DeathState { get; private set; }

    #endregion

    #region Components

    public Core Core { get; private set; }

    public Core_Movement Movement { get; private set; }
    private Core_Health health;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private CinemachineImpulseSource impulseSource;

    [Header("Scriptable Objects")]
    [SerializeField] private LuisStats miguelStats;

    [Header("Attack References")]
    public BeamWeapon[] skyBeams;
    public BeamWeapon upBeam;
    public BeamWeapon leftBeam;
    public BeamWeapon rightBeam;
    [SerializeField] private Projectile jumpArrows;
    [SerializeField] private Vector2 jumpArrowsDiagonal;
    //[field: SerializeField] public SerpentController serpentController { get; private set; }

    [field: Header("Waypoints")]
    [field: SerializeField] public Transform RightWaypoint;
    [field: SerializeField] public Transform LeftWaypoint;

    [field: Header("Sounds")]
    [field: SerializeField] public SoundID DeathSound { get; private set; }
    [field: SerializeField] public SoundID JumpSound { get; private set; }
    [field: SerializeField] public SoundID AimSound { get; private set; }
    [field: SerializeField] public SoundID ShootSound { get; private set; }

    [Header("Other Components")]
    [SerializeField] private Image bossAttackIndicator;


    #endregion

    #region Other Variables

    public enum ActionType { None, Normal, Jump, Special }
    public ActionType DesiredAction = ActionType.None;

    private Transform player;

    #endregion

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        Movement = Core.GetCoreComponent<Core_Movement>();
        health = Core.GetCoreComponent<Core_Health>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        anim = GetComponentInChildren<Animator>();

        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();

        StateMachine = new StateMachine();
        IdleState = new LuisST_Idle(this, StateMachine, miguelStats, anim, "Idle");
        JumpState = new LuisST_Jump(this, StateMachine, miguelStats, anim, "Jump");
        NormalAttack = new LuisST_NormalAttack(this, StateMachine, miguelStats, anim, "Attack");
        AirborneState = new LuisST_Airborne(this, StateMachine, miguelStats, anim, "InAir");
        SpecialAttack = new LuisST_SpecialAttack(this, StateMachine, miguelStats, anim, "Attack");
        DeathState = new LuisST_Death(this, StateMachine, miguelStats, anim, "Death");
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
        health.OnDeath += BossDeath;
    }

    private void OnDisable()
    {
        IdleState.UnsubscribeToEvents();
        JumpState.UnsubscribeToEvents();
        NormalAttack.UnsubscribeToEvents();
        AirborneState.UnsubscribeToEvents();
        SpecialAttack.UnsubscribeToEvents();

        animatorEvent.OnAnimationFinishedTrigger -= AnimationFinishedTrigger;
        health.OnDeath -= BossDeath;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive)
        {
            return;
        }

        Core.LogicUpdate();
        StateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive)
        {
            //movement.SetVelocityX(0);
            return;
        }

        StateMachine.CurrentState.OnFixedUpdate();
    }

    public void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    public void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }

    public void ShootJumpTripleVolley()
    {
        Projectile newNote1 = Instantiate(jumpArrows, transform.position, Quaternion.identity);
        Projectile newNote2 = Instantiate(jumpArrows, transform.position, Quaternion.identity);
        Projectile newNote3 = Instantiate(jumpArrows, transform.position, Quaternion.identity);

        Vector2 direction1 = Vector2.down;
        newNote1.transform.right = direction1;

        Vector2 direction2 = jumpArrowsDiagonal;
        newNote2.transform.right = direction2;

        Vector2 direction3 = new Vector2(jumpArrowsDiagonal.x * -1, jumpArrowsDiagonal.y);
        newNote3.transform.right = direction3;


        if (ShootSound.IsValid()) BroAudio.Play(ShootSound);
        newNote1.LaunchProjectile(direction1.normalized);
        newNote2.LaunchProjectile(direction2.normalized);
        newNote3.LaunchProjectile(direction3.normalized);
    }

    public void CheckFlip(Transform target)
    {
        int direction = 0;

        if (target.position.x > transform.position.x) direction = 1;
        else direction = -1;

        Movement.FlipCheck(direction);
    }

    public float GetHealth()
    {
        return Core.GetCoreComponent<Core_Health>().CurrentHealth;
    }

    private void BossDeath()
    {
        health.ToggleInvincibility(true);
        StateMachine.ChangeState(DeathState);
    }

    public void ToggleBossAttackIndicator(bool input)
    {
        bossAttackIndicator.gameObject.SetActive(input);
    }
}

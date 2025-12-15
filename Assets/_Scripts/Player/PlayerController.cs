using Ami.BroAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    #region State Machine Variables
    public StateMachine StateMachine { get; private set; }
    public PlayerST_Idle IdleState { get; private set; }
    public PlayerST_Move MoveState { get; private set; }
    public PlayerST_Jump JumpState { get; private set; }
    public PlayerST_Airborne AirborneState { get; private set; }
    public PlayerST_Land LandState { get; private set; }
    public PlayerST_Dash DashState { get; private set; }
    public PlayerST_Crouch CrouchState { get; private set; }
    public PlayerST_PrimeAttack PrimaryAttackState { get; private set; }
    public PlayerST_SecAttack SecondaryAttackState { get; private set; }
    public PlayerST_Stun StunState { get; private set; }
    public PlayerST_Death DeathState { get; private set; }

    #endregion

    #region Component References

    public Core Core { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public SpriteRenderer playerSprite { get; private set; }
    public BeatComboCounter BeatCombo { get; private set; }
    public AfterImage AfterImageController { get; private set; }

    [field: Header("Colliders")]
    [field: SerializeField] public BoxCollider2D PlayerPhysicsCollider { get; private set; }
    [field: SerializeField] public BoxCollider2D PlayerDamageCollider { get; private set; }

    [field: Header("Components")]
    [SerializeField] private PlayerStats playerData;
    [SerializeField] public PlayerWeapon weapon;
    [field: SerializeField] public ParticleSystem DoubleJumpParticles { get; private set; }

    private Core_CollisionSenses collisionSenses;
    private CharacterAnimatorEvent animatorEvent;
    private Core_Movement movement;
    private Core_Health health;

    [field: Header("Sounds")]
    [field: SerializeField] public SoundID JumpSound { get; private set; }
    [field: SerializeField] public SoundID DobleJumpSound { get; private set; }
    [field: SerializeField] public SoundID DashSound { get; private set; }
    [field: SerializeField] public SoundID BeatDashSound { get; private set; }
    [field: SerializeField] public SoundID StunSound { get; private set; }
    [field: SerializeField] public SoundID DeathSound { get; private set; }


    #endregion

    #region Other Variables

    public int StunBeatDuration { get; private set; }

    public Action OnStunEvent;

    private Vector2 workSpace;


    #endregion

    #region Unity Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        BeatCombo = GetComponent<BeatComboCounter>();

        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        collisionSenses = Core.GetCoreComponent<Core_CollisionSenses>();
        movement = Core.GetCoreComponent<Core_Movement>();
        health = Core.GetCoreComponent<Core_Health>();
        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();
        AfterImageController = GetComponentInChildren<AfterImage>();

        StateMachine = new StateMachine();
        IdleState = new PlayerST_Idle(this, playerData, StateMachine, Anim, "Idle");
        MoveState = new PlayerST_Move(this, playerData, StateMachine, Anim, "Move");
        JumpState = new PlayerST_Jump(this, playerData, StateMachine, Anim, "InAir");
        AirborneState = new PlayerST_Airborne(this, playerData, StateMachine, Anim, "InAir");
        LandState = new PlayerST_Land(this, playerData, StateMachine, Anim, "Land");
        DashState = new PlayerST_Dash(this, playerData, StateMachine, Anim, "Dash");
        CrouchState = new PlayerST_Crouch(this, playerData, StateMachine, Anim, "Crouch");
        //PrimaryAttackState = new PlayerST_PrimeAttack(this, playerData, StateMachine, Anim, "PrimeAttack", weapon);
        //SecondaryAttackState = new PlayerST_SecAttack(this, playerData, StateMachine, Anim, "SecAttack", weapon);
        StunState = new PlayerST_Stun(this, playerData, StateMachine, Anim, "Stun");
        DeathState = new PlayerST_Death(this, playerData, StateMachine, Anim, "Death");
    }

    private void Start()
    {
        weapon.InitializeWeapon(Core);
        DoubleJumpParticles.transform.SetParent(null);
        StateMachine.Initialize(IdleState);
        Core_Mana.ManaIsFull += EnergyFullAnimation;
        animatorEvent.OnAnimationFinishedTrigger += AnimationFinishedTrigger;
        PrimaryAttackState = new PlayerST_PrimeAttack(this, playerData, StateMachine, Anim, "PrimeAttack", weapon);
        SecondaryAttackState = new PlayerST_SecAttack(this, playerData, StateMachine, Anim, "SecAttack", weapon);
        health.OnDeath += ChangeToDeathState;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        Core.LogicUpdate();
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

    private void OnDisable()
    {
        IdleState.UnsubscribeToEvents();
        MoveState.UnsubscribeToEvents();
        JumpState.UnsubscribeToEvents();
        AirborneState.UnsubscribeToEvents();
        LandState.UnsubscribeToEvents();
        DashState.UnsubscribeToEvents();
        CrouchState.UnsubscribeToEvents();
        PrimaryAttackState.UnsubscribeToEvents();
        SecondaryAttackState.UnsubscribeToEvents();
        StunState.UnsubscribeToEvents();
        DeathState.UnsubscribeToEvents();
        Core_Mana.ManaIsFull -= EnergyFullAnimation;
        animatorEvent.OnAnimationFinishedTrigger -= AnimationFinishedTrigger;
        health.OnDeath -= ChangeToDeathState;
    }

    #endregion

    #region Other Functions

    public void SetColliderHeight(float height, BoxCollider2D collider)
    {
        Vector2 center = collider.offset;
        workSpace.Set(collider.size.x, height);

        center.y += (height - collider.size.y) / 2;

        collider.size = workSpace;
        collider.offset = center;
    }

    public void TryToStunPlayerIfGrounded(int value) 
    {
        //Debug.Log("TryToStun");
        if (!collisionSenses.Grounded) return;
        StunBeatDuration = value;
        StateMachine.ChangeState(StunState);
    } 

    public void StunPlayer(int value)
    {
        StunBeatDuration = value;
        StateMachine.ChangeState(StunState);
    }

    public void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    public void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }

    public float GetHealth()
    {
        throw new NotImplementedException();
    }

    private void EnergyFullAnimation()
    {
        Anim.SetTrigger("EnergyFull");
    }

    public bool ReleaseChargeAttack()
    {
        if (!weapon.UseWeaponOnReleaseInput) return false;

        if (GameInputManager.Instance.PrimaryAttackInputStop == 2) return true;
        else return false;
    }

    public void ActivateDoubleJumpParticle()
    {
        DoubleJumpParticles.transform.position = new Vector3(transform.position.x, transform.position.y - 1.1f, transform.position.z);
        DoubleJumpParticles.Play();
    }

    private void ChangeToDeathState()
    {
        StateMachine.ChangeState(DeathState);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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

    #endregion

    #region Component References

    public Core Core { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public SpriteRenderer playerSprite { get; private set; }
    public BeatComboCounter BeatCombo { get; private set; }
    [field: SerializeField] public BoxCollider2D[] PlayerCollider { get; private set; }

    [SerializeField] private PlayerStats playerData;
    [SerializeField] private PlayerWeapon weapon;
    //[field: SerializeField] public SoundLibraryObject playerLibrary { get; private set; }

    #endregion

    #region Other Variables

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

        if (PlayerCollider.Length != 2) Debug.LogError("Player got the wrong colliders.");

        weapon.InitializeWeapon(Core);

        StateMachine = new StateMachine();
        IdleState = new PlayerST_Idle(this, playerData, StateMachine, Anim, "Idle");
        MoveState = new PlayerST_Move(this, playerData, StateMachine, Anim, "Move");
        JumpState = new PlayerST_Jump(this, playerData, StateMachine, Anim, "InAir");
        AirborneState = new PlayerST_Airborne(this, playerData, StateMachine, Anim, "InAir");
        LandState = new PlayerST_Land(this, playerData, StateMachine, Anim, "Land");
        DashState = new PlayerST_Dash(this, playerData, StateMachine, Anim, "Dash");
        CrouchState = new PlayerST_Crouch(this, playerData, StateMachine, Anim, "Crouch");
        PrimaryAttackState = new PlayerST_PrimeAttack(this, playerData, StateMachine, Anim, "PrimeAttack", weapon);
        SecondaryAttackState = new PlayerST_SecAttack(this, playerData, StateMachine, Anim, "SecAttack", weapon);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
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
    }

    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        foreach (BoxCollider2D collider in PlayerCollider)
        {
            Vector2 center = collider.offset;
            workSpace.Set(collider.size.x, height);

            center.y += (height - collider.size.y) / 2;

            collider.size = workSpace;
            collider.offset = center;
        }
    }

    public void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    public void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }

    #endregion
}

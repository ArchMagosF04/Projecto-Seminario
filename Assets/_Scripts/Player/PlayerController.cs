using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public PlayerST_PrimaryAttack PrimaryAttackState { get; private set; }
    public PlayerST_SecondaryAttack SecondaryAttackState { get; private set; }

    #endregion

    #region Component References
    public Core Core { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerWeapon Weapon { get; private set; }
    [field: SerializeField] public BoxCollider2D[] PlayerCollider { get; private set; }

    [SerializeField] private PlayerData playerData;

    #endregion

    #region Other Variables

    private Vector2 workSpace;

    #endregion

    #region Unity Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        Weapon = GetComponentInChildren<PlayerWeapon>();

        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();

        if (PlayerCollider.Length != 2) Debug.LogError("Player got the wrong colliders.");

        StateMachine = new StateMachine();

        IdleState = new PlayerST_Idle(this, StateMachine, playerData, "Idle");
        MoveState = new PlayerST_Move(this, StateMachine, playerData, "Move");
        JumpState = new PlayerST_Jump(this, StateMachine, playerData, "InAir");
        AirborneState = new PlayerST_Airborne(this, StateMachine, playerData, "InAir");
        LandState = new PlayerST_Land (this, StateMachine, playerData, "Land");
        DashState = new PlayerST_Dash(this, StateMachine, playerData, "Dash");
        CrouchState = new PlayerST_Crouch(this, StateMachine, playerData, "Crouch");
        PrimaryAttackState = new PlayerST_PrimaryAttack(this, StateMachine, playerData, "Attack");
        SecondaryAttackState = new PlayerST_SecondaryAttack(this, StateMachine, playerData, "Attack", Weapon);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        Weapon.InitializeWeapon(PrimaryAttackState, SecondaryAttackState, Core);
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

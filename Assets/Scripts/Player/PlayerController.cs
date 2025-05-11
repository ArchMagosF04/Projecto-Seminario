using System.Collections;
using System.Collections.Generic;
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

    #endregion

    #region Component References
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public BoxCollider2D PlayerCollider { get; private set; }

    [SerializeField] private PlayerData playerData;

    #endregion

    #region Transforms
    [SerializeField] private Transform groundCheck;

    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }


    private Vector2 workSpace;

    #endregion

    #region Unity Functions
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        PlayerCollider = GetComponent<BoxCollider2D>();

        StateMachine = new StateMachine();

        IdleState = new PlayerST_Idle(this, StateMachine, playerData, "Idle");
        MoveState = new PlayerST_Move(this, StateMachine, playerData, "Move");
        JumpState = new PlayerST_Jump(this, StateMachine, playerData, "InAir");
        AirborneState = new PlayerST_Airborne(this, StateMachine, playerData, "InAir");
        LandState = new PlayerST_Land (this, StateMachine, playerData, "Land");
        DashState = new PlayerST_Dash(this, StateMachine, playerData, "Dash");
        CrouchState = new PlayerST_Crouch(this, StateMachine, playerData, "Crouch");
    }

    private void Start()
    {
        FacingDirection = 1;
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.OnFixedUpdate();
    }

    #endregion

    #region Set Functions
    
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workSpace = direction * velocity;
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocityX(float velocity)
    {
        workSpace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocityY(float velocity)
    {
        workSpace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    #endregion

    #region Check Functions

    public bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public void FlipCheck(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection) Flip();
    }

    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = PlayerCollider.offset;
        workSpace.Set(PlayerCollider.size.x, height);

        center.y += (height - PlayerCollider.size.y) / 2;

        PlayerCollider.size = workSpace;
        PlayerCollider.offset = center;
    }

    public void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    public void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }

    private void Flip()
    {
        FacingDirection *= -1;

        transform.Rotate(0f, 180f, 0f);
    }

    #endregion
}

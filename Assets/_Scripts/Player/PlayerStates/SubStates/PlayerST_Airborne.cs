using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Airborne : PlayerState
{
    //Inputs
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool dashInput;
    private bool primaryAttackInput;
    private bool secondaryAttackInput;

    //Checks
    private bool isGrounded;
    private bool coyoteTime;
    private bool isJumping;

    protected Core_Movement movement;

    protected Core_CollisionSenses collisionSenses;

    public PlayerST_Airborne(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenses.Grounded;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        CheckCoyoteTime();

        xInput = InputManager.Instance.NormInputX;
        jumpInput = InputManager.Instance.JumpInput;
        jumpInputStop = InputManager.Instance.JumpInputStop;
        dashInput = InputManager.Instance.DashInput;
        primaryAttackInput = InputManager.Instance.PrimaryAttackInput;
        secondaryAttackInput = InputManager.Instance.SecondaryAttackInput;

        CheckJumpMultipler();

        if (primaryAttackInput && controller.PrimaryAttackState.CanPerformAttack())
        {
            stateMachine.ChangeState(controller.PrimaryAttackState);
        }
        else if (secondaryAttackInput && controller.SecondaryAttackState.CanPerformSpecialAttack())
        {
            stateMachine.ChangeState(controller.SecondaryAttackState);
        }
        else if (isGrounded && movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(controller.LandState);
        }
        else if (jumpInput && controller.JumpState.CanJump())
        {
            stateMachine.ChangeState(controller.JumpState);
        }
        else if (dashInput && controller.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(controller.DashState);
        }
        else
        {
            movement.FlipCheck(xInput);
            movement.SetVelocityX(playerStats.MovementVelocity * xInput);

            controller.Anim.SetFloat("yVelocity", movement.CurrentVelocity.y);
        }
    }

    private void CheckJumpMultipler()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                movement.SetVelocityY(movement.CurrentVelocity.y * playerStats.VariableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerStats.CoyoteTime)
        {
            coyoteTime = false;
            controller.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void SetIsJumping() => isJumping = true;
}

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

    //Checks
    private bool isGrounded;
    private bool coyoteTime;
    private bool isJumping;

    public PlayerST_Airborne(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Grounded;
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

        xInput = controller.InputHandler.NormInputX;
        jumpInput = controller.InputHandler.JumpInput;
        jumpInputStop = controller.InputHandler.JumpInputStop;
        dashInput = controller.InputHandler.DashInput;

        CheckJumpMultipler();

        if (controller.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(controller.PrimaryAttackState);
        }
        else if (controller.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(controller.SecondaryAttackState);
        }

        else if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f)
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
            core.Movement.FlipCheck(xInput);
            core.Movement.SetVelocityX(playerData.movementVelocity * xInput);

            controller.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
        }
    }

    private void CheckJumpMultipler()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (core.Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            controller.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void SetIsJumping() => isJumping = true;
}

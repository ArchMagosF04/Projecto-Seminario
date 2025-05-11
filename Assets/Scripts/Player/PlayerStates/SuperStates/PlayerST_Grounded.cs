using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerST_Grounded : PlayerState
{
    protected int xInput;
    protected int yInput;

    private bool jumpInput;

    private bool dashInput;

    private bool isGrounded;

    public PlayerST_Grounded(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = controller.GroundCheck();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.JumpState.ResetAmountOfJumps();
        controller.DashState.ResetCanDash();
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

        xInput = controller.InputHandler.NormInputX;
        yInput = controller.InputHandler.NormInputY;
        jumpInput = controller.InputHandler.JumpInput;
        dashInput = controller.InputHandler.DashInput;

        if (jumpInput && controller.JumpState.CanJump())
        {
            stateMachine.ChangeState(controller.JumpState);
        }
        else if (!isGrounded)
        {
            controller.AirborneState.StartCoyoteTime();
            stateMachine.ChangeState(controller.AirborneState);
        }
        else if (dashInput && controller.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(controller.DashState);
        }
    }
}

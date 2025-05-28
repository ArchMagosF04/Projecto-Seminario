using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerST_Grounded : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool crouchInput;
    protected bool crouchInputStop;

    private bool jumpInput;

    private bool dashInput;

    private bool isGrounded;

    protected Movement Movement => movement ? movement : core.GetCoreComponent(ref movement); 
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses); //If its null then does the get the value on the right.
    private CollisionSenses collisionSenses;

    public PlayerST_Grounded(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses) 
        {
            isGrounded = CollisionSenses.Grounded;
        }
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
        crouchInput = controller.InputHandler.CrouchInput;
        crouchInputStop = controller.InputHandler.CrouchInputStop;

        if (controller.InputHandler.PrimaryAttackInput && controller.PrimaryAttackState.CanPerformAttack())
        {
            stateMachine.ChangeState(controller.PrimaryAttackState);
        }
        else if (controller.InputHandler.SecondaryAttackInput && controller.SecondaryAttackState.CanPerformSpecialAttack())
        {
            stateMachine.ChangeState(controller.SecondaryAttackState);
        }

        else if (jumpInput && controller.JumpState.CanJump())
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

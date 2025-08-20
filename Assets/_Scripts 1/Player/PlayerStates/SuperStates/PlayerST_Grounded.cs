using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerST_Grounded : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool crouchInput;
    protected bool crouchInputStop;

    private bool jumpInput;

    private bool dashInput;

    private bool isGrounded;

    private bool primaryAttack;
    private bool secondaryAttack;

    protected Core_Movement movement;

    protected Core_CollisionSenses collisionSenses;
 

    public PlayerST_Grounded(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (collisionSenses)
        {
            isGrounded = collisionSenses.Grounded;
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

        xInput = InputManager.Instance.NormInputX;
        yInput = InputManager.Instance.NormInputY;
        jumpInput = InputManager.Instance.JumpInput;
        dashInput = InputManager.Instance.DashInput;
        crouchInput = InputManager.Instance.CrouchInput;
        crouchInputStop = InputManager.Instance.CrouchInputStop;
        primaryAttack = InputManager.Instance.PrimaryAttackInput;
        secondaryAttack = InputManager.Instance.SecondaryAttackInput;

        if (primaryAttack && controller.PrimaryAttackState.CanPerformAttack())
        {
            stateMachine.ChangeState(controller.PrimaryAttackState);
        }
        else if (secondaryAttack && controller.SecondaryAttackState.CanPerformSpecialAttack())
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

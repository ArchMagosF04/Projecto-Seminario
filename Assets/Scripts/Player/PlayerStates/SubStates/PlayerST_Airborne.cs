using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Airborne : PlayerState
{
    private bool isGrounded;
    private int xInput;

    public PlayerST_Airborne(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
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

        if (isGrounded && controller.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(controller.LandState);
        }
        else
        {
            controller.FlipCheck(xInput);
            controller.SetVelocityX(playerData.movementVelocity * xInput);

            controller.Anim.SetFloat("yVelocity", controller.CurrentVelocity.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Grounded : PlayerState
{
    protected int xInput;

    private bool JumpInput;

    public PlayerST_Grounded(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
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
        JumpInput = controller.InputHandler.JumpInput;

        if (JumpInput)
        {
            controller.InputHandler.UseJumpInput();
            stateMachine.ChangeState(controller.JumpState);
        }
    }
}

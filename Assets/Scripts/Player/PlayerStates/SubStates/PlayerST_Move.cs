using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Move : PlayerST_Grounded
{
    public PlayerST_Move(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
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

        controller.FlipCheck(xInput);

        controller.SetVelocityX(playerData.movementVelocity * xInput);

        if (xInput == 0)
        {
            stateMachine.ChangeState(controller.IdleState);
        }
    }
}

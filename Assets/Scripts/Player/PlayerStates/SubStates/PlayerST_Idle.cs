using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Idle : PlayerST_Grounded
{
    public PlayerST_Idle(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.SetVelocityX(0f);
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

        if (xInput != 0)
        {
            stateMachine.ChangeState(controller.MoveState);
        }
    }
}

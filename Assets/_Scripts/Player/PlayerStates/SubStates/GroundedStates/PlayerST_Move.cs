using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Move : PlayerST_Grounded
{
    public PlayerST_Move(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (controller.Speaking)
        {
            OnExit();
        }
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

        movement.FlipCheck(xInput);

        movement.SetVelocityX(playerStats.MovementVelocity * xInput);

        if (xInput == 0 && !isExitingState)
        {
            stateMachine.ChangeState(controller.IdleState);
        }
    }
}

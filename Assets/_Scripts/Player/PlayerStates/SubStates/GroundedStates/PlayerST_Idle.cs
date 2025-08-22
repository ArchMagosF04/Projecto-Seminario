using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerST_Idle : PlayerST_Grounded
{
    public PlayerST_Idle(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        movement.SetVelocityX(0f);
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

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(controller.MoveState);
            }
            else if (crouchInput)
            {
                stateMachine.ChangeState(controller.CrouchState);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Land : PlayerST_Grounded
{
    public PlayerST_Land(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
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
            else if (isAnimationFinished)
            {
                stateMachine.ChangeState(controller.IdleState);
            }
        }
    }
}

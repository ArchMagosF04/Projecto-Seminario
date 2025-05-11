using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Land : PlayerST_Grounded
{
    public PlayerST_Land(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
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

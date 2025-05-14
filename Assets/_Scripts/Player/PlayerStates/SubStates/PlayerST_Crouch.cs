using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Crouch : PlayerST_Grounded
{
    public PlayerST_Crouch(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.InputHandler.UseCrouchInput();

        Movement?.SetVelocityZero();
        controller.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void OnExit()
    {
        base.OnExit();
        controller.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isExitingState && crouchInputStop)
        {
            stateMachine.ChangeState(controller.IdleState);
        }
    }
}

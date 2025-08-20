using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerST_Crouch : PlayerST_Grounded
{
    public PlayerST_Crouch(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        InputManager.Instance.UseCrouchInput();

        movement.SetVelocityZero();
        controller.SetColliderHeight(playerStats.CrouchColliderHeight);
    }

    public override void OnExit()
    {
        base.OnExit();
        controller.SetColliderHeight(playerStats.StandColliderHeight);
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

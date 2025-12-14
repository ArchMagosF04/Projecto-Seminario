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

        GameInputManager.Instance.UseCrouchInput();

        movement.SetVelocityZero();
        controller.SetColliderHeight(playerStats.CrouchPhysicsColliderHeight, controller.PlayerPhysicsCollider);
        controller.SetColliderHeight(playerStats.CrouchDamageColliderHeight, controller.PlayerDamageCollider);
    }

    public override void OnExit()
    {
        base.OnExit();
        controller.SetColliderHeight(playerStats.StandPhysicsColliderHeight, controller.PlayerPhysicsCollider);
        controller.SetColliderHeight(playerStats.StandDamageColliderHeight, controller.PlayerDamageCollider);
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

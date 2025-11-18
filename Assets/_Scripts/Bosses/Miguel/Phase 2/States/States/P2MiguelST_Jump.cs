using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class P2MiguelST_Jump : P2MiguelState
{
    private Core_Movement movement;
    private Core_CollisionSenses collisionSenses;

    public P2MiguelST_Jump(Phase2MiguelController controller, StateMachine stateMachine, P2MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (controller.DesiredAction == Phase2MiguelController.ActionType.Jump) 
            controller.DesiredAction = Phase2MiguelController.ActionType.None;

        //controller.PlaySound("Jump");
        PerformJump();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (!collisionSenses.Grounded) stateMachine.ChangeState(controller.AirborneState);
    }

    public void PerformJump()
    {
        Transform desiredJumpTarget;

        if (controller.transform.position.x >= 0) desiredJumpTarget = controller.LeftWaypoint;
        else desiredJumpTarget = controller.RightWaypoint;


        float distance = desiredJumpTarget.position.x - controller.transform.position.x;
        float mult = 1;

        controller.CheckFlip(desiredJumpTarget);

        //if (controller.DesiredJumpTarget.position.x != 0) mult = 1.15f;

        movement.JumpToLocation(distance * mult, stats.JumpForce);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GardelST_Jump : GardelState
{
    private Core_Movement movement;
    private Core_CollisionSenses collisionSenses;

    public GardelST_Jump(GardelController controller, StateMachine stateMachine, GardelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        PerformJump();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (!collisionSenses.Grounded) stateMachine.ChangeState(controller.AirborneState);
    }

    public void PerformJump()
    {
        float distance = controller.DesiredJumpTarget.position.x - controller.transform.position.x;
        float mult = 1;

        controller.CheckFlip(controller.DesiredJumpTarget);

        //if (controller.DesiredJumpTarget.position.x != 0) mult = 1.15f;

        movement.JumpToLocation(distance * mult, stats.JumpForce);
    }
}

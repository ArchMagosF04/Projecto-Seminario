using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaST_Jump : AnittaState
{
    private Core_Movement movement;
    private Core_CollisionSenses collisionSenses;

    public AnittaST_Jump(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //controller.PlaySound("Jump");
        PerformJump();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //if (!collisionSenses.Grounded) stateMachine.ChangeState(controller.AirborneState);
    }

    public void PerformJump()
    {
        float distance = controller.DesiredJumpTarget.position.x - controller.transform.position.x;
        float mult = 1;

        controller.CheckFlip(controller.DesiredJumpTarget);

        movement.JumpToLocation(distance * mult, stats.JumpForce);
    }
}

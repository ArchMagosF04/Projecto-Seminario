using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_Airborne : GardelState
{
    protected Core_Movement movement;

    protected Core_CollisionSenses collisionSenses;

    public GardelST_Airborne(GardelController controller, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (collisionSenses.Grounded && movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(controller.IdleState);
        }
        else
        {
            anim.SetFloat("yVelocity", movement.CurrentVelocity.y);
        }
    }
}

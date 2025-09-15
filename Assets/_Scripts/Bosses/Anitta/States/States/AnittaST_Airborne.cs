using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaST_Airborne : AnittaState
{
    protected Core_Movement movement;

    protected Core_CollisionSenses collisionSenses;

    public AnittaST_Airborne(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (collisionSenses.Grounded && movement.CurrentVelocity.y < 0.01f)
        {
            //controller.PlaySound("Landing");
            stateMachine.ChangeState(controller.IdleState);
        }
        else
        {
            anim.SetFloat("yVelocity", movement.CurrentVelocity.y);
        }
    }
}

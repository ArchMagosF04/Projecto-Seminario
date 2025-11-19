using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2MiguelST_Airborne : P2MiguelState
{
    protected Core_Movement movement;

    protected Core_CollisionSenses collisionSenses;

    public P2MiguelST_Airborne(Phase2MiguelController controller, StateMachine stateMachine, P2MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnEnter()
    {
        DoChecks();
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    public override void OnExit()
    {
        isExitingState = true;
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
            //anim.SetFloat("yVelocity", movement.CurrentVelocity.y);
        }
    }
}

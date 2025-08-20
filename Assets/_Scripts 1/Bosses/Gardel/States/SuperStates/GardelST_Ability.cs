using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GardelST_Ability : GardelState
{
    protected bool isAbilityDone;

    protected bool isGrounded;

    private Core_Movement movement;
    private Core_CollisionSenses collisionSenses;

    public GardelST_Ability(GardelController controller, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenses.Grounded;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        isAbilityDone = false;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (isAbilityDone)
        {
            if (isGrounded && movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(controller.IdleState);
            }
            else
            {
                //stateMachine.ChangeState(controller.AirborneState);
            }
        }
    }
}

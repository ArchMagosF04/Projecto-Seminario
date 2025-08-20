using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Ability : PlayerState
{
    protected bool isAbilityDone;

    protected bool isGrounded;

    protected Core_Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    private Core_Movement movement;

    protected Core_CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses); //If its null then does the get the value on the right.
    private Core_CollisionSenses collisionSenses;

    public PlayerST_Ability(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = CollisionSenses.Grounded;
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
            if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(controller.IdleState);
            }
            else
            {
                stateMachine.ChangeState(controller.AirborneState);
            }
        }
    }
}

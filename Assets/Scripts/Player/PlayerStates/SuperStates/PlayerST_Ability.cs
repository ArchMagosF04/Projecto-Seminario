using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Ability : PlayerState
{
    protected bool isAbilityDone;

    protected bool isGrounded;

    protected Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses); //If its null then does the get the value on the right.
    private CollisionSenses collisionSenses;

    public PlayerST_Ability(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
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

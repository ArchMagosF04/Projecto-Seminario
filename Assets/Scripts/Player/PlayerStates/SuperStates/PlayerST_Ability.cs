using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Ability : PlayerState
{
    protected bool isAbilityDone;

    private bool isGrounded;

    public PlayerST_Ability(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = controller.GroundCheck();
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
            if (isGrounded && controller.CurrentVelocity.y < 0.01f)
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

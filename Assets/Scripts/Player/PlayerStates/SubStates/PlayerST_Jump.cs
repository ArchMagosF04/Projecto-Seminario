using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Jump : PlayerST_Ability
{
    public PlayerST_Jump(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
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
    }
}

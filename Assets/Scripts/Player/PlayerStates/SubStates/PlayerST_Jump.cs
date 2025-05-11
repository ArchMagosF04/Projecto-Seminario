using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Jump : PlayerST_Ability
{
    private int amountOfJumpsLeft;

    public PlayerST_Jump(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.InputHandler.UseJumpInput();
        controller.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        DecreaseAmountOfJumpsLeft();
        controller.AirborneState.SetIsJumping();
    }

    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0) return true;

        return false;
    }

    public void ResetAmountOfJumps() => amountOfJumpsLeft = playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}

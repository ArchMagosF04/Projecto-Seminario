using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Jump : PlayerST_Ability
{
    private int amountOfJumpsLeft;

    public PlayerST_Jump(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        amountOfJumpsLeft = playerStats.AmountOfJumps;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.PlaySound("Jump");

        InputManager.Instance.UseJumpInput();
        Movement?.SetVelocityY(playerStats.JumpVelocity);
        isAbilityDone = true;
        DecreaseAmountOfJumpsLeft();
        controller.AirborneState.SetIsJumping();
    }

    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0) return true;

        return false;
    }

    public void ResetAmountOfJumps() => amountOfJumpsLeft = playerStats.AmountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}

using Ami.BroAudio;
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

        if (amountOfJumpsLeft <= 1 && controller.DobleJumpSound.IsValid()) BroAudio.Play(controller.DobleJumpSound);
        else if(controller.JumpSound.IsValid()) BroAudio.Play(controller.JumpSound);

        GameInputManager.Instance.UseJumpInput();
        Movement?.SetVelocityY(playerStats.JumpVelocity);
        isAbilityDone = true;
        DecreaseAmountOfJumpsLeft();
        controller.AirborneState.SetIsJumping();

        if (amountOfJumpsLeft <= 0)
        {
            controller.PlayOnBeatParticle();
            BeatManager.Instance.OnPlayerRhythmicAction();
            controller.BeatCombo.ResetDecayTimer();
            controller.ActivateDoubleJumpParticle();
            manaComponent.IncreaseMana(playerStats.ManaGainOnDoubleJump);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0) return true;      

        return false;
    }

    public void ResetAmountOfJumps() => amountOfJumpsLeft = playerStats.AmountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}

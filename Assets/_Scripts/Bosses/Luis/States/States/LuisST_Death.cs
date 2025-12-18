using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisST_Death : LuisState
{
    public LuisST_Death(LuisController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.MakePlayerInvincible();
        stateMachine.ToggleLockState(true);
        controller.Movement.SetVelocityZero();
        if (controller.DeathSound.IsValid()) BroAudio.Play(controller.DeathSound);
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        GameManager.Instance.OnGameWon();
    }
}

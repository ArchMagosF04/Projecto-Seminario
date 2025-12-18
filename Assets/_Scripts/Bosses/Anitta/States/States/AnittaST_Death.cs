using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaST_Death : AnittaState
{
    public AnittaST_Death(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.MakePlayerInvincible();
        stateMachine.ToggleLockState(true);
        //controller.Movement.SetVelocityZero();
        if (controller.DeathSound.IsValid()) BroAudio.Play(controller.DeathSound);
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        GameManager.Instance.OnGameWon();
    }
}

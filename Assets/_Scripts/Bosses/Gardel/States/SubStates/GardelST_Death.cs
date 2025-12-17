using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_Death : GardelState
{
    public GardelST_Death(GardelController controller, StateMachine stateMachine, GardelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
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

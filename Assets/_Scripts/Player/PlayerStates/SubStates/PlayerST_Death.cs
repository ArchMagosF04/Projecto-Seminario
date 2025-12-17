using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Death : PlayerState
{
    Core_Movement movement;

    public PlayerST_Death(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        movement.SetVelocityZero();
        if (controller.DeathSound.IsValid()) BroAudio.Play(controller.DeathSound);
        stateMachine.ToggleLockState(true);
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        GameManager.Instance.OnGameLost();
    }
}

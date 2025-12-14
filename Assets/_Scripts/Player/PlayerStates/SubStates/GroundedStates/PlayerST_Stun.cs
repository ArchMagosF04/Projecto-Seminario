using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerST_Stun : PlayerState
{
    private int beatTimer;

    private Core_Movement movement;
    private Core_Health health;

    public PlayerST_Stun(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        health = core.GetCoreComponent<Core_Health>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (controller.StunSound.IsValid()) BroAudio.Play(controller.StunSound);

        beatTimer = 0;
        movement.SetVelocityZero();

        BeatManager.Instance.intervals[0].OnBeatEvent += BeatTimer;
        health.OnDamageReceived += ForceExit;
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        health.OnDamageReceived -= ForceExit;
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    public override void OnExit()
    {
        base.OnExit();
        health.OnDamageReceived -= ForceExit;
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    private void BeatTimer()
    {
        beatTimer++;

        if (beatTimer >= controller.StunBeatDuration)
        {
            ForceExit();
        }
    }

    private void ForceExit()
    {
        stateMachine.ChangeState(controller.IdleState);
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }
}

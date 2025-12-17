using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_SpecialAttack : GardelState
{
    private int beatTimer;
    private bool attackPerformed;

    public GardelST_SpecialAttack(GardelController controller, StateMachine stateMachine, GardelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        attackPerformed = false;
        //controller.PlaySound("SpecialPrepare");

        beatTimer = 0;

        BeatManager.Instance.intervals[0].OnBeatEvent += BeatTimer;
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    public override void OnExit()
    {
        controller.ToggleBossAttackIndicator(false);
        base.OnExit();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    private void BeatTimer()
    {
        if (!GameManager.Instance.IsGameActive) return;

        beatTimer++;

        if (!attackPerformed && beatTimer + 1 >= stats.SpecialBeatsToWait)
        {
            controller.ToggleBossAttackIndicator(true);
            attackPerformed = true;
            controller.SpawnShout();
        }
        if (beatTimer > stats.SpecialBeatsToWait)
        {
            anim.SetTrigger("SpecialAttackBeat");
            int randomSound = Random.Range(1, 5);
            if (controller.SpecialAttackSound.IsValid()) BroAudio.Play(controller.SpecialAttackSound);
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        controller.LastAttackWasSpecial = true;
        controller.DesiredAction = GardelController.ActionType.None;
        stateMachine.ChangeState(controller.IdleState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_SpecialAttack : GardelState
{
    private int beatTimer;

    public GardelST_SpecialAttack(GardelController controller, StateMachine stateMachine, GardelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //if (controller.Speaking)
        //{
        //    OnExit();
        //}
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
        base.OnExit();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    private void BeatTimer()
    {
        beatTimer++;

        if (beatTimer > stats.SpecialBeatsToWait)
        {
            anim.SetTrigger("SpecialAttackBeat");
            controller.SpawnShout();
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

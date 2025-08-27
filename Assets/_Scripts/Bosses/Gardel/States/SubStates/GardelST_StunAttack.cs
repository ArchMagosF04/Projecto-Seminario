using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_StunAttack : GardelState
{
    private int beatTimer;

    public GardelST_StunAttack(GardelController controller, StateMachine stateMachine, GardelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
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

        if (beatTimer > stats.StunBeatsToWait)
        {
            anim.SetTrigger("StunAttackBeat");
            controller.StunningShout();

            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        PerformAction();
    }

    private void PerformAction()
    {
        if (controller.DesiredAction == GardelController.ActionType.Normal)
        {
            stateMachine.ChangeState(controller.NormalAttackState);
        }
        else if (controller.DesiredAction == GardelController.ActionType.Special)
        {
            stateMachine.ChangeState(controller.SpecialAttackState);
        }
    }
}

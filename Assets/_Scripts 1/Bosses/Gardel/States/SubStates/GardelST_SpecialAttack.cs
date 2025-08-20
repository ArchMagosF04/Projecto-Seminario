using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_SpecialAttack : GardelState
{
    private int beatsToWait = 1;
    private int beatTimer;

    public GardelST_SpecialAttack(GardelController controller, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stateMachine, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
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

        if (beatTimer > beatsToWait)
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

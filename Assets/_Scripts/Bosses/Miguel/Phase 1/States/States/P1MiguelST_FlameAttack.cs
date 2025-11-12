using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1MiguelST_FlameAttack : P1MiguelState
{
    private bool attackPerformed;

    public P1MiguelST_FlameAttack(Phase1MiguelController controller, StateMachine stateMachine, P1MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        attackPerformed = false;

        Debug.Log("FlameState");

        BeatManager.Instance.intervals[0].OnBeatEvent += PerformAttack;
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();

        BeatManager.Instance.intervals[0].OnBeatEvent -= PerformAttack;
    }

    public override void OnExit()
    {
        base.OnExit();
        BeatManager.Instance.intervals[0].OnBeatEvent -= PerformAttack;
    }

    public void PerformAttack()
    {
        if (attackPerformed) return;

        if (controller.Speaking) return;

        anim.SetTrigger("NormalAttackBeat");

        controller.FloorsManager.FlameAttack();

        attackPerformed = true;

        stateMachine.ChangeState(controller.IdleState);
    }

    //public override void AnimationFinishedTrigger()
    //{
    //    base.AnimationFinishedTrigger();

    //    if (attackPerformed)
    //    {
    //        controller.LastAttackWasSpecial = false;
    //        controller.DesiredAction = AnittaController.ActionType.None;
    //        stateMachine.ChangeState(controller.IdleState);
    //        return;
    //    }
    //}
}

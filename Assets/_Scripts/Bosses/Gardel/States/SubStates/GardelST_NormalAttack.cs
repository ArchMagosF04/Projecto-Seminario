using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_NormalAttack : GardelState
{
    private int attackCount = 0;

    public GardelST_NormalAttack(GardelController controller, StateMachine stateMachine, GardelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        attackCount = 0;

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
        if (stats.NumberOfAttacks == attackCount)
        {
            controller.LastAttackWasSpecial = false;
            controller.DesiredAction = GardelController.ActionType.None;
            stateMachine.ChangeState(controller.IdleState);
            return;
        }

        controller.CheckFlip(GameManager.Instance.PlayerTransform);

        anim.SetTrigger("NormalAttackBeat");

        controller.FireProjectile();

        attackCount++;
    }
}

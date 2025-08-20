using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelST_NormalAttack : GardelState
{
    private int numberOfAttacks = 3;
    private int attackCount = 0;

    public GardelST_NormalAttack(GardelController controller, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stateMachine, anim, animBoolName)
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
        if (numberOfAttacks == attackCount)
        {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1MiguelST_Idle : P1MiguelState
{
    private int beatTimer;

    private Core_Movement movement;

    private bool snakeAttacked;

    public P1MiguelST_Idle(Phase1MiguelController controller, StateMachine stateMachine, P1MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("IdleState");

        snakeAttacked = false;

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
        anim.ResetTrigger("IdleBeat");
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    private void BeatTimer()
    {
        beatTimer++;

        anim.SetTrigger("IdleBeat");
        //controller.PlaySound("FingerSnap");

        if (!snakeAttacked && beatTimer >= stats.BeatsSpentOnIdle / 2)
        {
            controller.FloorsManager.SerpentAttack();
            snakeAttacked = true;
        }

        if (beatTimer >= stats.BeatsSpentOnIdle && !controller.Speaking)
        {
            DecideAction();
        }
    }

    private void DecideAction()
    {
        if (controller.FloorsManager.AvailableFloors.Count > 0)
        {
            if (Random.value < stats.FlameAttackChance)
            {
                stateMachine.ChangeState(controller.FlameAttack);
                BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;

                return;
            }
        }

        //Change to normal Attack State
    }
}

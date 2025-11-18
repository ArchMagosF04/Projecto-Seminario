using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2MiguelST_Idle : P2MiguelState
{
    private int beatTimer;

    private Core_Movement movement;

    public P2MiguelST_Idle(Phase2MiguelController controller, StateMachine stateMachine, P2MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        movement.SetVelocityZero();

        beatTimer = 0;

        if (controller.DesiredAction != Phase2MiguelController.ActionType.None)
        {
            PerformAction();
            return;
        }

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
        Debug.Log(beatTimer);

        anim.SetTrigger("OnBeat");
        //controller.PlaySound("FingerSnap");

        if (beatTimer >= stats.BeatsSpentOnIdle && !controller.Speaking)
        {
            DecideAction();
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
        }
    }

    private void DecideAction()
    {
        if (controller.DesiredAction == Phase2MiguelController.ActionType.None)
        {
            if (Random.value <= stats.NormalAttackChance)
            {
                controller.DesiredAction = Phase2MiguelController.ActionType.Normal;
            }
            else if (Random.value <= stats.SpecialAttackChance)
            {
                controller.DesiredAction = Phase2MiguelController.ActionType.Special;
            }
            else
            {
                controller.DesiredAction = Phase2MiguelController.ActionType.Jump;
            }

            stateMachine.ChangeState(controller.JumpState);
        }
    }

    private void PerformAction()
    {
        stateMachine.ChangeState(controller.NormalAttack);

        if (controller.DesiredAction == Phase2MiguelController.ActionType.Normal)
        {
            stateMachine.ChangeState(controller.NormalAttack);
        }
        else if (controller.DesiredAction == Phase2MiguelController.ActionType.Special)
        {
            stateMachine.ChangeState(controller.SpecialAttack);
        }
        else if(controller.DesiredAction == Phase2MiguelController.ActionType.Jump)
        {
            stateMachine.ChangeState(controller.JumpState);
        }
    }
}

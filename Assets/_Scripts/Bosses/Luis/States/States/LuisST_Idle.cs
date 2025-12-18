using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisST_Idle : LuisState
{
    private int beatTimer;

    private Core_Movement movement;

    public LuisST_Idle(LuisController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        movement.SetVelocityZero();

        beatTimer = 0;

        if (controller.DesiredAction != LuisController.ActionType.None)
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
        if (!GameManager.Instance.IsGameActive) return;

        beatTimer++;

        anim.SetTrigger("IdleBeat");

        if (beatTimer >= stats.BeatsSpentOnIdle && GameManager.Instance.IsGameActive)
        {
            DecideAction();
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
        }
    }

    private void DecideAction()
    {
        if (controller.DesiredAction == LuisController.ActionType.None)
        {
            if (Random.value <= stats.NormalAttackChance)
            {
                controller.DesiredAction = LuisController.ActionType.Normal;
            }
            else if (Random.value <= stats.SpecialAttackChance)
            {
                controller.DesiredAction = LuisController.ActionType.Special;
            }
            else
            {
                controller.DesiredAction = LuisController.ActionType.Jump;
            }

            stateMachine.ChangeState(controller.JumpState);
        }
    }

    private void PerformAction()
    {
        stateMachine.ChangeState(controller.NormalAttack);

        if (controller.DesiredAction == LuisController.ActionType.Normal)
        {
            stateMachine.ChangeState(controller.NormalAttack);
        }
        else if (controller.DesiredAction == LuisController.ActionType.Special)
        {
            stateMachine.ChangeState(controller.SpecialAttack);
        }
        else if (controller.DesiredAction == LuisController.ActionType.Jump)
        {
            stateMachine.ChangeState(controller.JumpState);
        }
    }
}

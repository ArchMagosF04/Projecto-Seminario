using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaST_Idle : AnittaState
{
    private int beatTimer;

    private Core_Movement movement;

    public AnittaST_Idle(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        movement.SetVelocityZero();

        beatTimer = 0;

        if (controller.DesiredAction != AnittaController.ActionType.None)
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
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    private void BeatTimer()
    {
        beatTimer++;

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
        if (controller.DesiredAction == AnittaController.ActionType.None)
        {
            controller.DesiredAction = AnittaController.ActionType.Normal;



            //if (controller.LastAttackWasSpecial || Random.value >= stats.SpecialAttackChance + stats.SecretAttackChance)
            //{
            //    controller.DesiredAction = AnittaController.ActionType.Normal;  
            //}
            //else
            //{
            //    controller.DesiredAction = AnittaController.ActionType.Special;
            //}

            DecidePlatform();

            //stateMachine.ChangeState(controller.JumpState);
        }
    }

    private void PerformAction()
    {
        if (controller.DesiredAction == AnittaController.ActionType.Normal)
        {
            //stateMachine.ChangeState(controller.NormalAttackState);
        }
        //else if (controller.DesiredAction == AnittaController.ActionType.Special)
        //{
        //    stateMachine.ChangeState(controller.SpecialAttackState);
        //}
    }

    private void DecidePlatform()
    {

    }
}

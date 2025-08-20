using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GardelST_Idle : GardelState
{
    private int beatsSpentOnIdle = 3;
    private int beatTimer;

    private Core_Movement movement;

    public GardelST_Idle(GardelController controller, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        movement.SetVelocityZero();

        beatTimer = 0;

        if (controller.DesiredAction != GardelController.ActionType.None)
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

        controller.OnBeatAction();

        if (beatTimer == beatsSpentOnIdle)
        {
            DecideAction();
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
        }
    }

    private void DecideAction()
    {
        if (controller.DesiredAction == GardelController.ActionType.None)
        {
            if (controller.LastAttackWasSpecial || Random.value < 0.66f)
            {
                controller.DesiredAction = GardelController.ActionType.Normal;

                DecidePlatform();
            }
            else
            {
                controller.DesiredAction = GardelController.ActionType.Special;

                controller.DesiredJumpTarget = controller.stageCenter;
            }

            stateMachine.ChangeState(controller.JumpState);
        }
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

    private void DecidePlatform()
    {
        bool closeToRight = false;
        bool closeToLeft = false;

        if (Vector2.Distance(controller.transform.position, controller.leftPlatform.position) < 3.5f) closeToLeft = true;

        if (Vector2.Distance(controller.transform.position, controller.rightPlatform.position) < 3.5f) closeToRight = true;

        if (closeToLeft) controller.DesiredJumpTarget = controller.rightPlatform;
        if (closeToRight) controller.DesiredJumpTarget = controller.leftPlatform;

        if (!closeToRight && !closeToLeft)
        {
            if (Random.value < 0.5f) controller.DesiredJumpTarget = controller.leftPlatform;
            else controller.DesiredJumpTarget = controller.rightPlatform;
        }
    }
}

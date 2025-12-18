using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisST_Airborne : LuisState
{
    protected Core_Movement movement;

    protected Core_CollisionSenses collisionSenses;

    private bool hasAttacked;

    public LuisST_Airborne(LuisController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnEnter()
    {
        DoChecks();
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
        hasAttacked = false;

        BeatManager.Instance.intervals[0].OnBeatEvent += BeatTimer;
        controller.ToggleBossAttackIndicator(true);
    }

    public override void OnExit()
    {
        isExitingState = true;
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    private void BeatTimer()
    {
        if (!GameManager.Instance.IsGameActive) return;

        if (!hasAttacked && controller.transform.position.y > 1.5f)
        {
            hasAttacked = true;
            controller.ShootJumpTripleVolley();
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
            controller.ToggleBossAttackIndicator(false);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (collisionSenses.Grounded && movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(controller.IdleState);
        }
        else
        {
            //anim.SetFloat("yVelocity", movement.CurrentVelocity.y);
        }
    }
}

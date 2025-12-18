using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisST_NormalAttack : LuisState
{
    private int beatTimer;

    private bool attackPerformed;

    public LuisST_NormalAttack(LuisController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        beatTimer = 0;
        attackPerformed = false;

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
        controller.ToggleBossAttackIndicator(false);
        BeatManager.Instance.intervals[0].OnBeatEvent -= PerformAttack;
        controller.DesiredAction = LuisController.ActionType.None;
        anim.ResetTrigger("AttackBeat");
    }

    public void PerformAttack()
    {
        if (attackPerformed)
        {
            stateMachine.ChangeState(controller.IdleState);
            return;
        }

        beatTimer++;

        if (beatTimer == (1 + stats.BeatsBeforeNormalAttack))
        {
            controller.ToggleBossAttackIndicator(true);

            controller.upBeam.SetAim();

            if (controller.transform.position.x > 0) controller.leftBeam.SetAim();
            else controller.rightBeam.SetAim();

        }

        if (beatTimer >= (3 + stats.BeatsBeforeNormalAttack))
        {
            anim.SetTrigger("AttackBeat");

            controller.upBeam.FireBeam();

            if (controller.transform.position.x > 0) controller.leftBeam.FireBeam();
            else controller.rightBeam.FireBeam();

            controller.SnakeController.ShootHighProjectile();

            attackPerformed = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2MiguelST_SpecialAttack : P2MiguelState
{
    private int beatTimer;

    private bool attackPerformed;

    public P2MiguelST_SpecialAttack(Phase2MiguelController controller, StateMachine stateMachine, P2MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
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
        BeatManager.Instance.intervals[0].OnBeatEvent -= PerformAttack;
        controller.DesiredAction = Phase2MiguelController.ActionType.None;
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

        if (beatTimer == (1 + stats.BeatsBeforeSpecialAttack))
        {
            foreach(BeamWeapon beam in controller.bodyBeams)
            {
                beam.SetAim();
            }
        }

        if (beatTimer >= 2 + stats.BeatsBeforeSpecialAttack)
        {
            anim.SetTrigger("AttackBeat");

            foreach (BeamWeapon beam in controller.bodyBeams)
            {
                beam.FireBeam();
            }

            attackPerformed = true;
        }
    }
}

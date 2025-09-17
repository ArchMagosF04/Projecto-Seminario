using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaST_NormalAttack : AnittaState
{
    private bool attackPerformed;

    public AnittaST_NormalAttack(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

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
    }

    public void PerformAttack()
    {
        if (attackPerformed) return;

        controller.CheckFlip(GameManager.Instance.PlayerInstance.transform);

        anim.SetTrigger("NormalAttackBeat");

        //int randomSound = Random.Range(1, 5);

        //controller.PlaySound("MusicNote-" + randomSound.ToString());

        controller.FireProjectile();

        attackPerformed = true;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        if (attackPerformed)
        {
            controller.LastAttackWasSpecial = false;
            controller.DesiredAction = AnittaController.ActionType.None;
            stateMachine.ChangeState(controller.IdleState);
            return;
        }
    }
}

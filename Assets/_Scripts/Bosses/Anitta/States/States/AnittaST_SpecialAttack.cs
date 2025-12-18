using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AnittaST_SpecialAttack : AnittaState
{
    private bool attackPerformed;

    private int beatTimer;

    public AnittaST_SpecialAttack(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.ToggleBossAttackIndicator(true);

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
    }

    public void PerformAttack()
    {
        beatTimer++;
        if (attackPerformed || beatTimer <= stats.SpecialBeatsToWait) return;
        if (!GameManager.Instance.IsGameActive) return;

        controller.CheckFlip(GameManager.Instance.PlayerInstance.transform);

        anim.SetTrigger("SpecialAttackBeat");

        //int randomSound = Random.Range(1, 5);

        //controller.PlaySound("MusicNote-" + randomSound.ToString());

        controller.FireWave();

        attackPerformed = true;

        controller.ToggleBossAttackIndicator(false);
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        if (attackPerformed)
        {
            controller.LastAttackWasSpecial = true;
            controller.DesiredAction = AnittaController.ActionType.None;
            stateMachine.ChangeState(controller.IdleState);
            return;
        }
    }
}

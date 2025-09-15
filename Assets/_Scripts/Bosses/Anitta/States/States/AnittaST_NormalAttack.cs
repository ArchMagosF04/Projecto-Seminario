using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaST_NormalAttack : AnittaState
{
    public AnittaST_NormalAttack(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

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
        //if (stats.NumberOfAttacks == attackCount)
        //{
        //    controller.LastAttackWasSpecial = false;
        //    controller.DesiredAction = GardelController.ActionType.None;
        //    stateMachine.ChangeState(controller.IdleState);
        //    return;
        //}

        //controller.CheckFlip(GameManager.Instance.PlayerInstance.transform);

        //anim.SetTrigger("NormalAttackBeat");

        //int randomSound = Random.Range(1, 5);

        //controller.PlaySound("MusicNote-" + randomSound.ToString());

        //controller.FireProjectile();

        //attackCount++;
    }
}

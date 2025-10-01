using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaST_Teleport : AnittaState
{
    private bool hasTeleportedOut;
    private int beatTimer;


    public AnittaST_Teleport(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {

    }

    public override void OnEnter()
    {
        if (controller.Speaking) OnExit();
        base.OnEnter();        
        hasTeleportedOut = false;
        beatTimer = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatCounter;
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatCounter;
    }

    private void BeatCounter()
    {
        beatTimer++;

        if (beatTimer >= stats.BeatsBeforeReappearance)
        {
            controller.CheckFlip(GameManager.Instance.PlayerInstance.transform);
            anim.SetTrigger("TPBeat");
            controller.TargetIndicator.enabled = false;
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatCounter;
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        if (!hasTeleportedOut)
        {
            hasTeleportedOut = true;
            controller.DamageCollider.enabled = false;
            controller.transform.position = controller.DesiredJumpTarget.position;

            controller.TargetIndicator.enabled = true;

            BeatManager.Instance.intervals[0].OnBeatEvent += BeatCounter;
        }
        else
        {
            //Restore hitbox
            controller.DamageCollider.enabled = true;

            stateMachine.ChangeState(controller.IdleState);
        }
    }
}

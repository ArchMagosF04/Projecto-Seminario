using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerST_Stun : PlayerState
{
    private int beatTimer;

    private Core_Movement movement;

    public PlayerST_Stun(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        beatTimer = 0;
        movement.SetVelocityZero();
        Debug.Log("Player Stun");
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

        if (beatTimer >= controller.StunBeatDuration)
        {
            stateMachine.ChangeState(controller.IdleState);
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
        }
    }
}

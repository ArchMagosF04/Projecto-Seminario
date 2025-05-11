using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController controller;
    protected StateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;

    public PlayerState(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void OnEnter()
    {
        DoChecks();
        controller.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate()
    {
        DoChecks();
    }

    public virtual void OnExit()
    {
        controller.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishedTrigger() => isAnimationFinished = true;
}

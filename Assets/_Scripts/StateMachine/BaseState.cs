using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected Animator anim;
    protected StateMachine stateMachine;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;

    public BaseState(StateMachine stateMachine, Animator anim, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.anim = anim;
        this.animBoolName = animBoolName;
    }

    public virtual void OnEnter()
    {
        DoChecks();
        anim.SetBool(animBoolName, true);
        startTime = Time.time;
        //Debug.Log(animBoolName);
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
        anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishedTrigger() => isAnimationFinished = true;

    public virtual void SubscribeToEvents() { }
    public virtual void UnsubscribeToEvents() { }
}

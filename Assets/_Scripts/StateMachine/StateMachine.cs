using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StateMachine
{
    public BaseState CurrentState { get; private set; }

    private bool lockState = false;

    public void Initialize(BaseState startingState)
    {
        CurrentState = startingState;
        CurrentState.OnEnter();
    }

    public void ChangeState(BaseState newState)
    {
        if (lockState) return;

        CurrentState.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }

    public void ToggleLockState(bool input) => lockState = input;
}

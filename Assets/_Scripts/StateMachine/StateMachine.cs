using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StateMachine
{
    public BaseState CurrentState { get; private set; }

    public void Initialize(BaseState startingState)
    {
        CurrentState = startingState;
        CurrentState.OnEnter();
    }

    public void ChangeState(BaseState newState)
    {
        CurrentState.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }
}

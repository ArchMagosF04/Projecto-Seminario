using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericFSM<T>
{
    IState<T> currentState;

    public GenericFSM(IState<T> initialState)
    {
        SetInitialState(initialState);
    }

    public void SetInitialState(IState<T> state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void OnUpdate()
    {
        currentState.Execute();
    }

    public void OnFixedUpdate()
    {
        currentState.FixedExecute();
    }

    public void ChangeState(T condition)
    {
        IState<T> newState = currentState.GetTransition(condition);
        if (newState == null) return;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public IState<T> GetCurrentState()
    {
        return currentState;
    }
    
}

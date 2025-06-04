using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> : IState<T>
{
    Dictionary<T, IState<T>> transitions = new Dictionary<T, IState<T>>();
    public virtual void Enter()
    {
        
    }

    public virtual void Execute()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void FixedExecute()
    {
        
    }

    public void AddTransition(T condition, IState<T> newState)
    {
        transitions[condition] = newState;
    }

    public IState<T> GetTransition(T condition)
    {
        if (!transitions.ContainsKey(condition)) return null;
        return transitions[condition];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : IState
{
    //Dictionary<T, IState> transitions = new Dictionary<T, IState>();
    public virtual void Enter()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Execute()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Exit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void FixedExecute()
    {
        throw new System.NotImplementedException();
    }

    //public void AddTransition(T condition, IState<T> newState)
    //{
    //    transitions[condition] = newState;
    //}

    //public IState<T> GetTransition(T condition)
    //{
    //    if (!transitions.ContainsKey(condition)) return null;
    //    return transitions[condition];
    //}
}

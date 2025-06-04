using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IState<T>
{
    void Enter();
    void Execute();
    void FixedExecute();
    void Exit();

    public void AddTransition(T condition, IState<T> newState);

    public IState<T> GetTransition(T condition);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Player : IState
{
    private PlayerMovement player;
    private StateMachine stateMachine;

    public Run_Player(PlayerMovement player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }
    void IState.OnEnter()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.Transitions()
    {
        throw new System.NotImplementedException();
    }
}
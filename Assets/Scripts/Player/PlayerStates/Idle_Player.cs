using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Player : IState
{
    private PlayerMovement player;
    private StateMachine stateMachine;

    public Idle_Player(PlayerMovement player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    void IState.OnEnter()
    {
        player.animator.SetBool("IsIdle", true);
    }

    void IState.OnUpdate()
    {

    }

    void IState.OnFixedUpdate()
    {

    }

    void IState.OnExit()
    {
        player.animator.SetBool("IsIdle", false);
    }

    void IState.Transitions()
    {
        if (InputManager.Movement != Vector2.zero)
        {
            stateMachine.ChangeState(player.RunState);
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    protected PlayerController controller;
    protected Core core;
    protected PlayerStats playerStats;

    public PlayerState(PlayerController controller, PlayerStats stats, StateMachine stateMachine,Animator anim, string animBoolName) : base(stateMachine, anim, animBoolName)
    {
        this.controller = controller;
        core = controller.Core;
        playerStats = stats;
    }
}

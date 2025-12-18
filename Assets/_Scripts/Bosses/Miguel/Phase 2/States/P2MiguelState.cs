using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2MiguelState : BaseState
{
    protected Phase2MiguelController controller;
    protected Core core;
    protected LuisStats stats;

    public P2MiguelState(Phase2MiguelController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(stateMachine, anim, animBoolName)
    {
        this.controller = controller;
        core = controller.Core;
        this.stats = stats;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1MiguelState : BaseState
{
    protected Phase1MiguelController controller;
    protected Core core;
    protected P1MiguelStats stats;

    public P1MiguelState(Phase1MiguelController controller, StateMachine stateMachine, P1MiguelStats stats, Animator anim, string animBoolName) : base(stateMachine, anim, animBoolName)
    {
        this.controller = controller;
        core = controller.Core;
        this.stats = stats;
    }
}

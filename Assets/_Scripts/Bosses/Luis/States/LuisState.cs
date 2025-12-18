using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisState : BaseState
{
    protected LuisController controller;
    protected Core core;
    protected LuisStats stats;

    public LuisState(LuisController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(stateMachine, anim, animBoolName)
    {
        this.controller = controller;
        core = controller.Core;
        this.stats = stats;
    }
}

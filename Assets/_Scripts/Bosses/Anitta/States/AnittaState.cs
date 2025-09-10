using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaState : BaseState
{
    protected AnittaController controller;
    protected Core core;
    protected AnittaStats stats;

    public AnittaState(AnittaController controller, StateMachine stateMachine, AnittaStats stats, Animator anim, string animBoolName) : base(stateMachine, anim, animBoolName)
    {
        this.controller = controller;
        core = controller.Core;
        this.stats = stats;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelState : BaseState
{
    protected GardelController controller;
    protected Core core;

    public GardelState(GardelController controller, StateMachine stateMachine, Animator anim, string animBoolName) : base(stateMachine, anim, animBoolName)
    {
        this.controller = controller;
        core = controller.Core;
    }
}

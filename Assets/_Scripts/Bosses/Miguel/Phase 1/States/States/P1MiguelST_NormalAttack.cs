using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1MiguelST_NormalAttack : P1MiguelState
{
    private Core_Movement movement;

    public P1MiguelST_NormalAttack(Phase1MiguelController controller, StateMachine stateMachine, P1MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
    }

    public override void OnEnter()
    {
        base.OnEnter();


    }
}

using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisST_Jump : LuisState
{
    private Core_Movement movement;
    private Core_CollisionSenses collisionSenses;

    public LuisST_Jump(LuisController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void OnEnter()
    {
        DoChecks();
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;

        if (controller.DesiredAction == LuisController.ActionType.Jump)
            controller.DesiredAction = LuisController.ActionType.None;

        //if (Random.value > 0.5) controller.SnakeController.ShootHighProjectile();
        //else controller.SnakeController.ShootLowProjectile();

        if (controller.JumpSound.IsValid()) BroAudio.Play(controller.JumpSound);
        
        PerformJump();
    }

    public override void OnExit()
    {
        isExitingState = true;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (!collisionSenses.Grounded) stateMachine.ChangeState(controller.AirborneState);
    }

    public void PerformJump()
    {
        Transform desiredJumpTarget;

        if (controller.transform.position.x >= 0) desiredJumpTarget = controller.LeftWaypoint;
        else desiredJumpTarget = controller.RightWaypoint;


        float distance = desiredJumpTarget.position.x - controller.transform.position.x;
        float mult = 1;

        controller.CheckFlip(desiredJumpTarget);

        //if (controller.DesiredJumpTarget.position.x != 0) mult = 1.15f;

        movement.JumpToLocation(distance * mult, stats.JumpForce);
    }
}

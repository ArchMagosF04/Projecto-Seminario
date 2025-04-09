using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerMovement player;
    protected StateMachine playerStateMachine;

    public PlayerState(PlayerMovement player, StateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    //public virtual void AnimationTriggerEvent(PlayerMovement.AnimationTriggerType type) { }

}

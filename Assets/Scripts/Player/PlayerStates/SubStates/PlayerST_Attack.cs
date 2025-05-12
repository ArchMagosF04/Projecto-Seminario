using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Attack : PlayerST_Ability
{
    private Weapon weapon;

    private int xInput;

    private float velocityToSet;
    private bool setVelocity;

    private bool shoudCheckFlip;

    public PlayerST_Attack(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        setVelocity = false;

        weapon.EnterWeapon();
    }

    public override void OnExit()
    {
        base.OnExit();

        weapon.ExitWeapon();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        xInput = controller.InputHandler.NormInputX;

        if (shoudCheckFlip)
        {
            core.Movement.FlipCheck(xInput);
        }

        if (setVelocity)
        {
            core.Movement.SetVelocityX(velocityToSet * core.Movement.FacingDirection);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this);
    }

    public void SetPlayerVelocity(float velocity)
    {
        core.Movement.SetVelocityX(velocity * core.Movement.FacingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shoudCheckFlip = value;
    }

    #region Animation Triggers

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        isAbilityDone = true;
    }

    #endregion
}



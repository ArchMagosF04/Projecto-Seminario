using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Attack : PlayerST_Ability
{
    private Weapon weapon;

    public int XInput { get; private set; }
    public int YInput { get; private set; }

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

        XInput = controller.InputHandler.NormInputX;
        YInput = controller.InputHandler.NormInputY;

        if (shoudCheckFlip)
        {
            core.Movement.FlipCheck(XInput);
        }

        if (setVelocity)
        {
            core.Movement.SetVelocityX(velocityToSet * core.Movement.FacingDirection);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, core);
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



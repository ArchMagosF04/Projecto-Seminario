using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_PrimaryAttack : PlayerST_Ability
{
    private Weapon weapon;

    public int XInput { get; private set; }
    public int YInput { get; private set; }

    private float velocityToSet;
    private bool setVelocity;

    private bool shoudCheckFlip;

    public PlayerST_PrimaryAttack(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        setVelocity = false;

        XInput = controller.InputHandler.NormInputX;
        YInput = controller.InputHandler.NormInputY;

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
            Movement?.FlipCheck(XInput);
        }

        if (setVelocity)
        {
            Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, core);
    }

    public void SetPlayerVelocity(float velocity)
    {
        Movement?.SetVelocityX(velocity * Movement.FacingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shoudCheckFlip = value;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        isAbilityDone = true;
    }
}



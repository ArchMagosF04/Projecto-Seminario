using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_PrimaryAttack : PlayerST_Ability
{
    private PlayerWeapon weapon;

    public int XInput { get; private set; }
    public int YInput { get; private set; }

    private bool shoudCheckFlip;

    public PlayerST_PrimaryAttack(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName, PlayerWeapon weapon) : base(controller, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
        weapon.OnExit += ExitHandler;
    }

    public override void OnEnter()
    {
        base.OnEnter();

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
    }

    public void SetFlipCheck(bool value)
    {
        shoudCheckFlip = value;
    }

    private void ExitHandler()
    {
        AnimationFinishedTrigger();

        isAbilityDone = true;
    }
}



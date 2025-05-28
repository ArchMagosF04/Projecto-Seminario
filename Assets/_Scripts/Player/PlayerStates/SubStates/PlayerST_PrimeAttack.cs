using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_PrimeAttack : PlayerST_Ability
{
    public int XInput { get; private set; }
    public int YInput { get; private set; }

    private PlayerWeapon weapon;

    public PlayerST_PrimeAttack(PlayerController controller, StateMachine stateMachine, PlayerData playerData,
        string animBoolName, PlayerWeapon weapon) : base(controller, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
        weapon.OnExit += ExitHandler;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.InputHandler.UsePrimaryAttackInput();

        XInput = controller.InputHandler.NormInputX;
        YInput = controller.InputHandler.NormInputY;

        weapon.Enter(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        XInput = controller.InputHandler.NormInputX;
        YInput = controller.InputHandler.NormInputY;
    }

    public bool CanPerformAttack()
    {
        //TO DO

        return true;
    }

    private void ExitHandler()
    {
        AnimationFinishedTrigger();
        isAbilityDone = true;
    }
}



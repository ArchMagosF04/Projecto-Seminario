using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_SecondaryAttack : PlayerST_Ability
{
    private PlayerWeapon weapon;


    public PlayerST_SecondaryAttack(PlayerController controller, StateMachine stateMachine, PlayerData playerData,
        string animBoolName, PlayerWeapon weapon) : base(controller, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
        weapon.OnExit += ExitHandler;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        weapon.EnterWeapon();
        controller.InputHandler.UsePrimaryAttackInput();
    }

    public bool CanSpecialAttack()
    {
        return false;
    }

    private void ExitHandler()
    {
        AnimationFinishedTrigger();
        isAbilityDone = true;
    }
}

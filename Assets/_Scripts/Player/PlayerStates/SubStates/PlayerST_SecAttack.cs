using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_SecAttack : PlayerST_Ability
{
    private PlayerWeapon weapon;

    public PlayerST_SecAttack(PlayerController controller, StateMachine stateMachine, PlayerData playerData,
        string animBoolName, PlayerWeapon weapon) : base(controller, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
        weapon.OnExit += ExitHandler;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        controller.InputHandler.UseSecondaryAttackInput();

        weapon.Enter(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public bool CanPerformSpecialAttack()
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

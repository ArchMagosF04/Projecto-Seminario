using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_SecAttack : PlayerST_Ability
{
    private PlayerWeapon weapon;
    private Core_Mana manaComponent;

    public PlayerST_SecAttack(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName, PlayerWeapon weapon) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        this.weapon = weapon;
        manaComponent = controller.Core.GetCoreComponent<Core_Mana>();
        weapon.OnExit += ExitHandler;
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        weapon.OnExit -= ExitHandler;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        InputManager.Instance.UseSecondaryAttackInput();
        BeatManager.Instance.OnPlayerRhythmicAction();

        manaComponent.UseMana();

        weapon.ExecuteSpecialAttack();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public bool CanPerformSpecialAttack()
    {
        if (manaComponent.isManaFull) return true;

        InputManager.Instance.UseSecondaryAttackInput();

        return false;
    }

    private void ExitHandler()
    {
        AnimationFinishedTrigger();
        isAbilityDone = true;
    }
}

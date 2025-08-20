using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_PrimeAttack : PlayerST_Ability
{
    private PlayerWeapon weapon;

    public PlayerST_PrimeAttack(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName, PlayerWeapon weapon) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        this.weapon = weapon;
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

        InputManager.Instance.UsePrimaryAttackInput();
        BeatManager.Instance.OnPlayerRhythmicAction();

        weapon.ExecuteBasicAttack();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
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

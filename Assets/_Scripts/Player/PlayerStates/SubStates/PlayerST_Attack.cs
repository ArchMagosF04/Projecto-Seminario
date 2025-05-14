using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Attack : PlayerST_Ability
{
    private Weapon weapon;
    public PlayerST_Attack(PlayerController controller, StateMachine stateMachine, PlayerData playerData,
        string animBoolName, Weapon weapon) : base(controller, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
        weapon.OnExit += ExitHandler;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        weapon.Enter();
    }

    private void ExitHandler()
    {
        AnimationFinishedTrigger();
        isAbilityDone = true;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Attack : PlayerST_Ability
{
    public int XInput { get; private set; }
    public int YInput { get; private set; }

    private PlayerWeapon weapon;

    public PlayerST_Attack(PlayerController controller, StateMachine stateMachine, PlayerData playerData,
        string animBoolName, PlayerWeapon weapon) : base(controller, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
        weapon.OnExit += ExitHandler;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        XInput = controller.InputHandler.NormInputX;
        YInput = controller.InputHandler.NormInputY;

        weapon.Enter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        XInput = controller.InputHandler.NormInputX;
        YInput = controller.InputHandler.NormInputY;
    }

    private void ExitHandler()
    {
        AnimationFinishedTrigger();
        isAbilityDone = true;
    }
}



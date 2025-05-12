using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Dash : PlayerST_Ability
{
    public bool CanDash { get; private set; }
    private bool isHolding;
    private bool dashInputStop;

    private float lastDashTime;

    private Vector2 dashDirection;

    private Vector2 dashDirectionInput;

    public PlayerST_Dash(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        CanDash = false;
        controller.InputHandler.UseDashInput();

        isHolding = true;

        dashDirection = Vector2.right * core.Movement.FacingDirection;

        Time.timeScale = playerData.holdTimeScale; //Delete Later
        startTime = Time.unscaledTime; //Delete Later
    }

    public override void OnExit()
    {
        base.OnExit();

        if (core.Movement.CurrentVelocity.y > 0)
        {
            core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.dashEndYMultipler);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isExitingState)
        {
            if (isHolding) //Delete Later
            {
                dashDirectionInput = controller.InputHandler.DashDirectionInput;
                dashInputStop = controller.InputHandler.DashInputStop;

                if (dashDirectionInput != Vector2.zero)
                {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, dashDirection);

                if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;
                    core.Movement.FlipCheck(Mathf.RoundToInt(dashDirection.x));
                    controller.RB.drag = playerData.drag;
                    core.Movement.SetVelocity(playerData.dashVelocity, dashDirection);
                }
            }
            else
            {
                core.Movement.SetVelocity(playerData.dashVelocity, dashDirection);

                if (Time.time >= startTime + playerData.dashTime)
                {
                    controller.RB.drag = 0f;
                    isAbilityDone = true;
                    lastDashTime = Time.time;
                }
            }
        }
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash() => CanDash = true;

}

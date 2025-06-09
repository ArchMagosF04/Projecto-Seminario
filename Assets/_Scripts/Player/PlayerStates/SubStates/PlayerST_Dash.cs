using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerST_Dash : PlayerST_Ability
{
    public bool CanDash { get; private set; }
    private bool isHolding;
    private bool dashInputStop;

    private bool isInvincible;
    private bool canInvincibleDash = true;
    private int beatsToInvincibleDash = 0;

    private float lastDashTime;

    private Vector2 dashDirection;

    private Vector2 dashDirectionInput;

    private DamageReceiver damageReceiver;
    private KnockBackReceiver knockBackReceiver;

    private Color spriteColor;

    public PlayerST_Dash(PlayerController controller, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
        damageReceiver = controller.Core.GetCoreComponent<DamageReceiver>();
        knockBackReceiver = controller.Core.GetCoreComponent<KnockBackReceiver>();
        spriteColor = controller.playerSprite.color;
    }

    public override void SubscribeToEvents()
    {
        base.SubscribeToEvents();
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        BeatManager.Instance.OneBeat.OnBeatEvent -= InvincibleDashCooldown;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        CanDash = false;
        controller.InputHandler.UseDashInput();

        PlayerBeatManager.Instance.OnBeatAction();

        CheckInvincibleDash();

        isHolding = true;

        dashDirection = Vector2.right * Movement.FacingDirection;

        Time.timeScale = playerData.holdTimeScale; //Delete Later
        startTime = Time.unscaledTime; //Delete Later
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetInvincibleDash();

        if (Movement.CurrentVelocity.y > 0)
        {
            Movement.SetVelocityY(Movement.CurrentVelocity.y * playerData.dashEndYMultipler);
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

                //if (dashDirectionInput != Vector2.zero)
                //{
                //    dashDirection = dashDirectionInput;
                //    dashDirection.Normalize();
                //}

                //float angle = Vector2.SignedAngle(Vector2.right, dashDirection); //This is for a theoretical indicator for the dash direction.

                if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;
                    Movement.FlipCheck(Mathf.RoundToInt(dashDirection.x));
                    controller.RB.drag = playerData.drag;
                    Movement.SetVelocity(playerData.dashVelocity, dashDirection);
                }
            }
            else
            {
                Movement.SetVelocity(playerData.dashVelocity, dashDirection);

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

    private void CheckInvincibleDash()
    {
        if (canInvincibleDash && BeatManager.Instance.OneBeat.BeatGrace)
        {
            Debug.Log("Invincible Dash");
            damageReceiver.ToggleInvincibility(true);
            knockBackReceiver.ToggleHyperArmor(true);
            spriteColor.a = 0.5f;
            controller.playerSprite.color = spriteColor;
            isInvincible = true;
        }
    }

    private void ResetInvincibleDash()
    {
        if (isInvincible)
        {
            isInvincible = false;
            damageReceiver.ToggleInvincibility(false);
            knockBackReceiver.ToggleHyperArmor(false);
            spriteColor.a = 1f;
            controller.playerSprite.color = spriteColor;
            canInvincibleDash = false;
            BeatManager.Instance.OneBeat.OnBeatEvent += InvincibleDashCooldown;
        }
    }

    private void InvincibleDashCooldown()
    {
        if (!canInvincibleDash)
        {
            beatsToInvincibleDash++;
            if (beatsToInvincibleDash == 2)
            {
                canInvincibleDash = true;
                beatsToInvincibleDash = 0;
                Debug.Log("IDash Enable");
                controller.ActivateDashCooldownIndicator();
                BeatManager.Instance.OneBeat.OnBeatEvent -= InvincibleDashCooldown;
            }
        }
    }
}

using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST_Dash : PlayerST_Ability
{
    public bool CanDash { get; private set; }

    private bool isInvincible;
    private bool canInvincibleDash = true;
    private int beatsToInvincibleDash = 0;

    private float lastDashTime;
    private float dashCooldown;

    private Vector2 dashDirection;

    private Core_Health healthComponent;
    private Core_Knockback knockbackComponent;

    public PlayerST_Dash(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        dashCooldown = (60f / BeatManager.Instance.BPM) - stats.DashCooldownReduction;
        healthComponent = core.GetCoreComponent<Core_Health>();
        knockbackComponent = core.GetCoreComponent<Core_Knockback>();
    }

    public override void SubscribeToEvents()
    {
        base.SubscribeToEvents();
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        //BeatManager.Instance.intervals[0].OnBeatEvent -= InvincibleDashCooldown;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Movement.FlipCheck(GameInputManager.Instance.NormInputX);

        CanDash = false;
        GameInputManager.Instance.UseDashInput();

        BeatManager.Instance.OnPlayerRhythmicAction();

        if (BeatManager.Instance.BeatGracePeriod)
        {
            manaComponent.IncreaseMana(playerStats.ManaGainOnBeatDash);
            controller.BeatCombo.ResetDecayTimer();
            if (controller.BeatDashSound.IsValid()) BroAudio.Play(controller.BeatDashSound);
        }
        else if (controller.DashSound.IsValid()) BroAudio.Play(controller.DashSound);

        CheckInvincibleDash();

        dashDirection = Vector2.right * Movement.FacingDirection;
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetInvincibleDash();

        if (Movement.CurrentVelocity.y > 0)
        {
            Movement.SetVelocityY(Movement.CurrentVelocity.y * playerStats.DashEndYMultiplier);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isExitingState)
        {
            Movement.SetVelocity(playerStats.DashVelocity, dashDirection);

            if (Time.time >= startTime + playerStats.DashTime)
            {
                controller.RB.drag = 0f;
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
        }
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + dashCooldown;
    }

    public void ResetCanDash() => CanDash = true;

    private void CheckInvincibleDash()
    {
        if (BeatManager.Instance.BeatGracePeriod)
        {
            if (canInvincibleDash)
            {
                healthComponent.ToggleInvincibility(true);
                knockbackComponent.ToggleHyperArmor(true);
                isInvincible = true;
                controller.AfterImageController.Activate(true);
            }
        }
    }

    private void ResetInvincibleDash()
    {
        if (isInvincible)
        {
            isInvincible = false;
            healthComponent.ToggleInvincibility(false);
            knockbackComponent.ToggleHyperArmor(false);
            controller.AfterImageController.Activate(false);
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
                //controller.ActivateDashCooldownIndicator();
                //BeatManager.Instance.OneBeat.OnBeatEvent -= InvincibleDashCooldown;
            }
        }
    }
}

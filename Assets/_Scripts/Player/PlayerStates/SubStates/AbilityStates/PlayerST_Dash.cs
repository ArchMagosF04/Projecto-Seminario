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



    //private DamageReceiver damageReceiver;
    //private KnockBackReceiver knockBackReceiver;

    public PlayerST_Dash(PlayerController controller, PlayerStats stats, StateMachine stateMachine, Animator anim, string animBoolName) : base(controller, stats, stateMachine, anim, animBoolName)
    {
        dashCooldown = (60f / BeatManager.Instance.BPM) - stats.DashCooldownReduction;
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

        CanDash = false;
        InputManager.Instance.UseDashInput();

        BeatManager.Instance.OnPlayerRhythmicAction();

        if (BeatManager.Instance.BeatGracePeriod) controller.BeatCombo.ResetDecayTimer();

        //CheckInvincibleDash();

        dashDirection = Vector2.right * Movement.FacingDirection;
    }

    public override void OnExit()
    {
        base.OnExit();

        //ResetInvincibleDash();

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
                //damageReceiver.ToggleInvincibility(true);
                //knockBackReceiver.ToggleHyperArmor(true);
                //spriteColor.a = 0.5f;
                //controller.playerSprite.color = spriteColor;
                //isInvincible = true;
                //SoundManager.Instance.CreateSound().WithSoundData(controller.playerLibrary.soundData[0]).WithRandomPitch().Play();
            }
        }
    }

    private void ResetInvincibleDash()
    {
        if (isInvincible)
        {
            isInvincible = false;
            //damageReceiver.ToggleInvincibility(false);
            //knockBackReceiver.ToggleHyperArmor(false);
            //spriteColor.a = 1f;
            //controller.playerSprite.color = spriteColor;
            //canInvincibleDash = false;
            //BeatManager.Instance.OneBeat.OnBeatEvent += InvincibleDashCooldown;
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

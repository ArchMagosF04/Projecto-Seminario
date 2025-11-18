using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1MiguelST_NormalAttack : P1MiguelState
{
    private Core_Movement movement;

    private bool aimPerformed;

    private bool attackPerformed;

    private int beatTimer;

    private Transform playerTarget;

    public P1MiguelST_NormalAttack(Phase1MiguelController controller, StateMachine stateMachine, P1MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        movement = core.GetCoreComponent<Core_Movement>();
        playerTarget = GameManager.Instance.PlayerInstance.transform;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        movement.SetVelocityZero();
        movement.CanSetVelocity = false;

        attackPerformed = false;
        aimPerformed = false;
        beatTimer = 0;

        BeatManager.Instance.intervals[0].OnBeatEvent += BeatTimer;
    }

    private void AimBeam()
    {
        Vector3 randPos = new Vector3(playerTarget.position.x + Random.Range(-1, 1), playerTarget.position.y + Random.Range(-1, 1));

        controller.beamWeapon.SetAimWithTarget(randPos);
    }

    private void FireBeam()
    {
        controller.beamWeapon.FireBeam();
    }

    private void BeatTimer()
    {
        if (attackPerformed) stateMachine.ChangeState(controller.IdleState);

        beatTimer++;

        if (beatTimer > stats.BeatsBeforeNormalAttack)
        {
            if (!aimPerformed)
            {
                aimPerformed = true;
                AimBeam();
            }
            else if (!attackPerformed)
            {
                FireBeam();
                attackPerformed = true;
            }
        }
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    public override void OnExit()
    {
        base.OnExit();
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
        movement.CanSetVelocity = true;
    }
}

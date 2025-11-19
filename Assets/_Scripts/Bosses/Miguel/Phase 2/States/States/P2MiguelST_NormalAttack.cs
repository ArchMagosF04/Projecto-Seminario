using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2MiguelST_NormalAttack : P2MiguelState
{
    private int beatTimer;

    private bool attackPerformed;

    private Transform targetPlayer;

    private int beamIndex;

    public P2MiguelST_NormalAttack(Phase2MiguelController controller, StateMachine stateMachine, P2MiguelStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {
        targetPlayer = GameManager.Instance.PlayerInstance.transform;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        beatTimer = 0;
        beamIndex = 0;
        attackPerformed = false;

        BeatManager.Instance.intervals[0].OnBeatEvent += PerformAttack;
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();

        BeatManager.Instance.intervals[0].OnBeatEvent -= PerformAttack;
    }

    public override void OnExit()
    {
        base.OnExit();
        BeatManager.Instance.intervals[0].OnBeatEvent -= PerformAttack;
        controller.DesiredAction = Phase2MiguelController.ActionType.None;
        anim.ResetTrigger("AttackBeat");
    }

    public void PerformAttack()
    {
        beatTimer++;

        if (beatTimer <= stats.BeatsBeforeNormalAttack) return;

        if (attackPerformed)
        {
            stateMachine.ChangeState(controller.IdleState);
            return;
        }

        if (beamIndex >= controller.skyBeams.Length)
        {
            anim.SetTrigger("AttackBeat");

            foreach (BeamWeapon beam in controller.skyBeams)
            {
                beam.FireBeam();
            }

            attackPerformed = true;

            return;
        }

        BeamWeapon selectedBeam = controller.skyBeams[beamIndex];
        selectedBeam.transform.position = new Vector2(targetPlayer.position.x + Random.Range(-1f, 1f), selectedBeam.transform.position.y);
        selectedBeam.SetAim();
        beamIndex++;
    }
}

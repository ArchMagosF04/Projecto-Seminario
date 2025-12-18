using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisST_SpecialAttack : LuisState
{
    private int beatTimer;

    private bool attackPerformed;

    private int beamIndex;

    private int exception1;
    private int exception2;
    private int exception3;

    private List<BeamWeapon> chosenBeams = new List<BeamWeapon>();

    public LuisST_SpecialAttack(LuisController controller, StateMachine stateMachine, LuisStats stats, Animator anim, string animBoolName) : base(controller, stateMachine, stats, anim, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        beatTimer = 0;
        beamIndex = 0;
        exception1 = 0;
        exception2 = 0;
        exception3 = 0;
        attackPerformed = false;
        chosenBeams.Clear();

        ChooseBeamsToSkip();
        BeatManager.Instance.intervals[2].OnBeatEvent += PerformAttack;
    }

    public override void UnsubscribeToEvents()
    {
        base.UnsubscribeToEvents();

        BeatManager.Instance.intervals[2].OnBeatEvent -= PerformAttack;
    }

    public override void OnExit()
    {
        base.OnExit();
        BeatManager.Instance.intervals[2].OnBeatEvent -= PerformAttack;
        controller.DesiredAction = LuisController.ActionType.None;
        anim.ResetTrigger("AttackBeat");
    }

    public void PerformAttack()
    {
        beatTimer++;

        if (beatTimer <= stats.BeatsBeforeSpecialAttack) return;

        if (attackPerformed)
        {
            stateMachine.ChangeState(controller.IdleState);
            return;
        }

        if (beamIndex >= controller.skyBeams.Length)
        {
            anim.SetTrigger("AttackBeat");

            foreach (BeamWeapon beam in chosenBeams)
            {
                beam.FireBeam();
            }

            attackPerformed = true;

            return;
        }

        if (beamIndex == exception1 || beamIndex == exception2 || beamIndex == exception3) beamIndex++;
        if (beamIndex == exception1 || beamIndex == exception2 || beamIndex == exception3) beamIndex++;
        if (beamIndex == exception1 || beamIndex == exception2 || beamIndex == exception3) beamIndex++;

        BeamWeapon selectedBeam = controller.skyBeams[beamIndex];
        selectedBeam.SetAim();
        chosenBeams.Add(selectedBeam);

        beamIndex++;
    }


    private void ChooseBeamsToSkip()
    {
        exception1 = Random.Range(0, controller.skyBeams.Length);

        do
        {
            exception2 = Random.Range(0, controller.skyBeams.Length);
        } while (exception2 == exception1);

        do
        {
            exception3 = Random.Range(0, controller.skyBeams.Length);
        } while (exception3 == exception1 || exception3 == exception2);
    }
}

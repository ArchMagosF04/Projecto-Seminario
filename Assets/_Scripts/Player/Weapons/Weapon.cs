using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected SO_WeaponData weaponData;

    protected Animator weaponAnimator;

    protected PlayerST_PrimaryAttack state;

    protected Core core;

    protected int attackCounter;

    protected Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    private Movement movement;

    protected virtual void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        if (attackCounter >= weaponData.amountofAttacks)
        {
            attackCounter = 0;
        }

        weaponAnimator.SetBool("Attack", true);

        weaponAnimator.SetInteger("AttackCounter", attackCounter);
    }

    public virtual void ExitWeapon()
    {
        weaponAnimator.SetBool("Attack", false);

        attackCounter++;

        gameObject.SetActive(false);
    }

    #region Animation Triggers

    public virtual void AnimationFinishedTrigger()
    {
        state.AnimationFinishedTrigger();
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationActionTrigger()
    {

    }

    #endregion

    public void InitializeWeapon(PlayerST_PrimaryAttack state, Core core)
    {
        this.state = state;
        this.core = core;
    }
}

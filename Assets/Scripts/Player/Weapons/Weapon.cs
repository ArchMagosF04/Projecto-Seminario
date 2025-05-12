using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SO_WeaponData weaponData;

    protected Animator weaponAnimator;

    protected PlayerST_Attack state;

    protected int attackCounter;

    protected virtual void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
    }

    protected void Start()
    {
        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        if (attackCounter >= weaponData.movementSpeed.Length)
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

    #endregion

    public void InitializeWeapon(PlayerST_Attack state)
    {
        this.state = state;
    }
}

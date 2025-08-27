using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    #region Events

    public event Action OnSpecialEnter;
    public event Action OnBasicEnter;
    public event Action OnExit;

    #endregion

    #region Component References
    public WeaponAnimationEventHandler EventHandler { get; private set; }
    public BeatComboCounter beatCombo { get; private set; }
    public Core Core { get; private set; }
    public Core_Mana manaComponent { get; private set; }
    public Core_Movement movementComponent { get; private set; }

    protected Animator anim;

    #endregion

    #region Other Variables

    public bool isOnBeat { get; private set; } = false;

    #endregion

    protected virtual void Awake()
    {
        beatCombo = GetComponentInParent<BeatComboCounter>();
        anim = GetComponentInChildren<Animator>();
        EventHandler = GetComponentInChildren<WeaponAnimationEventHandler>();
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
    }

    protected virtual void OnEnable()
    {
        EventHandler.OnFinish += Exit;
    }

    protected virtual void OnDisable()
    {
        EventHandler.OnFinish -= Exit;
        EventHandler.OnStopMovement -= HandleStopMovement;
    }

    public virtual void InitializeWeapon(Core core)
    {
        Core = core;
        manaComponent = Core.GetCoreComponent<Core_Mana>();
        movementComponent = Core.GetCoreComponent<Core_Movement>();

        EventHandler.OnStopMovement += HandleStopMovement;
    }

    public virtual void ExecuteBasicAttack()
    {
        if (BeatManager.Instance.BeatGracePeriod) isOnBeat = true;

        if (isOnBeat)
        {
            print("Attack on beat");
        }
        

        OnBasicEnter?.Invoke();

        anim.SetBool("Active", true);
    }

    public virtual void ExecuteSpecialAttack()
    {
        if (BeatManager.Instance.BeatGracePeriod) isOnBeat = true;

        OnSpecialEnter?.Invoke();

        anim.SetBool("Special", true);
        anim.SetBool("Active", true);
    }

    protected virtual void Exit()
    {
        anim.SetBool("Active", false);
        anim.SetBool("Special", false);

        isOnBeat = false;

        OnExit?.Invoke();
    }

    protected virtual void HandleStopMovement()
    {
        movementComponent.SetVelocityZero();
    }
}

using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public event Action OnEnter;
    public event Action OnExit;

    [SerializeField] protected SO_WeaponData weaponData;

    public Core Core { get; private set; }
    public WeaponAnimationEventHandler EventHandler { get; private set; }
    public int CurrentAttackIndex { get; private set; }

    protected Animator weaponAnimator;

    protected PlayerST_PrimaryAttack primaryAttackState;
    protected PlayerST_SecondaryAttack secondaryAttackState;

    

    protected WP_Component[] components;

    protected virtual void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();

        EventHandler = GetComponentInChildren<WeaponAnimationEventHandler>();

        components = GetComponents<WP_Component>();

        foreach (var component in components)
        {
            component.Init(this, weaponData);
        }

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.OnFinish += ExitWeapon;
        EventHandler.OnTurnOnFlip += TurnOnFlip;
        EventHandler.OnTurnOffFlip += TurnOffFlip;
    }

    private void OnDisable()
    {
        EventHandler.OnFinish -= ExitWeapon;
        EventHandler.OnTurnOnFlip -= TurnOnFlip;
        EventHandler.OnTurnOffFlip -= TurnOffFlip;
    }

    public void SetCurrentAttackIndex(int index)
    {
        this.CurrentAttackIndex = index;

        if (CurrentAttackIndex >= weaponData.AttackDetails.Length)
        {
            CurrentAttackIndex = 0;
        }
    }

    public void InitializeWeapon(PlayerST_PrimaryAttack state1, PlayerST_SecondaryAttack state2, Core core)
    {
        primaryAttackState = state1;
        secondaryAttackState = state2;
        this.Core = core;
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        weaponAnimator.SetBool("Attack", true);

        OnEnter?.Invoke();
    }

    public virtual void ExitWeapon()
    {
        weaponAnimator.SetBool("Attack", false);

        gameObject.SetActive(false);

        OnExit?.Invoke();
    }

    private void TurnOnFlip()
    {
        primaryAttackState.SetFlipCheck(true);
    }

    private void TurnOffFlip()
    {
        primaryAttackState.SetFlipCheck(false);
    }
}

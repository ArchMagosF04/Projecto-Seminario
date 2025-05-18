using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public event Action OnEnter;
    public event Action OnExit;

    [SerializeField] protected SO_WeaponData weaponData;

    public Core Core { get; private set; }
    public WeaponAnimationEventHandler EventHandler { get; private set; }
    public WeaponAttackDetails currentAttackDetails { get; private set; }

    protected Animator weaponAnimator;

    protected PlayerST_PrimaryAttack primaryAttackState;
    protected PlayerST_SecondaryAttack secondaryAttackState;

    

    protected WP_Component[] components;

    protected virtual void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();

        EventHandler = GetComponentInChildren<WeaponAnimationEventHandler>();

        components = GetComponents<WP_Component>();

        gameObject.SetActive(false);
    }

    protected void Start()
    {
        foreach (var component in components)
        {
            component.Init(this, weaponData);
        }
    }

    private void OnEnable()
    {
        EventHandler.OnFinish += ExitWeapon;
    }

    private void OnDisable()
    {
        EventHandler.OnFinish -= ExitWeapon;
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
}

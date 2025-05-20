using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    #region Events

    public event Action OnEnter;
    public event Action OnExit;

    #endregion

    #region Component References
    [field: SerializeField] public SO_WeaponData Data { get; private set; }
    public WeaponAnimationEventHandler EventHandler { get; private set; }
    public Core Core { get; private set; }
    public PlayerST_Attack state { get; private set; }

    private Animator anim;
    private Timer attackCounterResetTimer;

    #endregion

    #region Other Variables

    [SerializeField] private float attackCounterResetCooldown;
    public int CurrentAttackCounter
    {
        get => currentAttackCounter;

        private set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0 : value;
    }

    private int currentAttackCounter;

    #endregion

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        EventHandler = GetComponentInChildren<WeaponAnimationEventHandler>();

        attackCounterResetTimer = new Timer(attackCounterResetCooldown);
    }

    private void Update()
    {
        attackCounterResetTimer.Tick();
    }

    private void OnEnable()
    {
        EventHandler.OnFinish += Exit;
        attackCounterResetTimer.OnTimerDone += ResetAttackCounter;
    }

    private void OnDisable()
    {
        EventHandler.OnFinish -= Exit;
        attackCounterResetTimer.OnTimerDone -= ResetAttackCounter;
    }

    public void SetCore(Core core) => Core = core;
    public void SetState(PlayerST_Attack state) => this.state = state;

    private void ResetAttackCounter() => CurrentAttackCounter = 0;
    public void ModifyAttackCounter(int amount) => CurrentAttackCounter = amount;

    public void Enter()
    {
        //Debug.Log($"{transform.name} enter");

        attackCounterResetTimer.StopTimer();

        OnEnter?.Invoke();

        anim.SetBool("Active", true);
        anim.SetInteger("Counter", currentAttackCounter);
    }

    private void Exit()
    {
        anim.SetBool("Active", false);

        attackCounterResetTimer.StartTimer();

        OnExit?.Invoke();
    }
}

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
    public PlayerST_PrimeAttack state { get; private set; }

    private Animator anim;
    private Timer attackCounterResetTimer;

    #endregion

    #region Other Variables

    [SerializeField] private float attackCounterResetCooldown;

    public bool isSpecialAttack { get; private set; }
    public bool isOnBeat { get; private set; }

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
    public void SetState(PlayerST_PrimeAttack state) => this.state = state;

    private void ResetAttackCounter() => CurrentAttackCounter = 0;
    public void ModifyAttackCounter(int amount) => CurrentAttackCounter = amount;

    public void Enter(bool isSpecial)
    {
        //Debug.Log($"{transform.name} enter");

        attackCounterResetTimer.StopTimer();

        isSpecialAttack = isSpecial;

        if(BeatManager.Instance.OneBeat.BeatGrace) isOnBeat = true;

        OnEnter?.Invoke();

        anim.SetBool("Special", isSpecial);
        anim.SetBool("Active", true);
        anim.SetInteger("Counter", currentAttackCounter);
    }

    private void Exit()
    {
        anim.SetBool("Active", false);
        anim.SetBool("Special", false);
        isSpecialAttack = false;
        isOnBeat = false;

        attackCounterResetTimer.StartTimer();

        OnExit?.Invoke();
    }
}

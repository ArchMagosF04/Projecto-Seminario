using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float attackCounterResetCooldown;

    public SO_WeaponData Data { get; private set; }

    public int CurrentAttackCounter
    {
        get => currentAttackCounter;

        private set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0 : value;
    }

    public event Action OnEnter;
    public event Action OnExit;

    private Animator anim;
    public GameObject BaseGameObject { get; private set; }
    public GameObject WeaponSpriteGameObject { get; private set; }

    public WeaponAnimationEventHandler EventHandler { get; private set; }

    public Core Core { get; private set; }

    private int currentAttackCounter;

    private Timer attackCounterResetTimer;

    private void Awake()
    {
        BaseGameObject = transform.Find("Base").gameObject;
        WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;

        anim = BaseGameObject.GetComponent<Animator>();
        EventHandler = BaseGameObject.GetComponent<WeaponAnimationEventHandler>();

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

    public void SetCore(Core core)
    {
        Core = core;
    }

    public void SetData(SO_WeaponData data)
    {
        Data = data;
    }

    private void ResetAttackCounter() => CurrentAttackCounter = 0;

    public void Enter()
    {
        //Debug.Log($"{transform.name} enter");

        attackCounterResetTimer.StopTimer();

        anim.SetBool("Active", true);
        anim.SetInteger("Counter", currentAttackCounter);

        OnEnter?.Invoke();
    }

    private void Exit()
    {
        anim.SetBool("Active", false);

        CurrentAttackCounter++;
        attackCounterResetTimer.StartTimer();

        OnExit?.Invoke();
    }
}


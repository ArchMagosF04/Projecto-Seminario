using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GardelController : MonoBehaviour, ISpeaker
{
    #region State Machine Varibles
    public StateMachine StateMachine {  get; private set; }
    public GardelST_Idle IdleState { get; private set; }
    public GardelST_Jump JumpState { get; private set; }
    public GardelST_Airborne AirborneState { get; private set; }
    public GardelST_NormalAttack NormalAttackState { get; private set; }
    public GardelST_SpecialAttack SpecialAttackState { get; private set; }
    public GardelST_StunAttack StunAttackState { get; private set; }
    #endregion

    #region Component References
    public Core Core { get; private set; }
    
    private Core_Health health;
    private Core_Movement movement;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private CinemachineImpulseSource impulseSource;

    [Header("Scriptable Objects")]
    [SerializeField] private GardelStats gardelStats;
    [SerializeField] private SoundLibraryObject soundLibrary;

    [field: Header("Boss Waypoints")]
    [field: SerializeField] public Transform rightPlatform { get; private set; }
    [field: SerializeField] public Transform leftPlatform { get; private set; }
    [field: SerializeField] public Transform stageCenter { get; private set; }

    #endregion

    #region Other Variables
    [Header("Status")]
    public bool LastAttackWasSpecial;
    public Transform DesiredJumpTarget;
    
    public enum ActionType { None, Normal, Special }
    public ActionType DesiredAction = ActionType.None;

    private bool speaking;

    public bool Speaking { get { return speaking; } }


    #endregion

    #region Unity Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        health = Core.GetCoreComponent<Core_Health>();
        movement = Core.GetCoreComponent<Core_Movement>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
        soundLibrary.Initialize();

        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();

        StateMachine = new StateMachine();
        IdleState = new GardelST_Idle(this, StateMachine, gardelStats, anim, "Idle");
        JumpState = new GardelST_Jump(this, StateMachine, gardelStats, anim, "InAir");
        AirborneState = new GardelST_Airborne(this, StateMachine, gardelStats, anim, "InAir");
        NormalAttackState = new GardelST_NormalAttack(this, StateMachine, gardelStats, anim, "NormalAttack");
        SpecialAttackState = new GardelST_SpecialAttack(this, StateMachine, gardelStats, anim, "SpecialAttack");
        StunAttackState = new GardelST_StunAttack(this, StateMachine, gardelStats, anim, "StunAttack");

        StateMachine.Initialize(IdleState);
    }

    private void OnEnable()
    {
        animatorEvent.OnAnimationFinishedTrigger += AnimationFinishedTrigger;
    }

    private void OnDisable()
    {
        IdleState.UnsubscribeToEvents();
        JumpState.UnsubscribeToEvents();
        AirborneState.UnsubscribeToEvents();
        NormalAttackState.UnsubscribeToEvents();
        SpecialAttackState.UnsubscribeToEvents();
        StunAttackState.UnsubscribeToEvents();

        animatorEvent.OnAnimationFinishedTrigger -= AnimationFinishedTrigger;
    }

    private void Update()
    {        
        StateMachine.CurrentState.OnUpdate();        
    }

    private void FixedUpdate()
    {
            StateMachine.CurrentState.OnFixedUpdate();        
    }

    #endregion

    #region Other Functions

    public void PlaySound(string name)
    {
        SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.Library[name]).Play();
    }

    public void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    public void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }

    public bool IsAtHalfHealth()
    {
        if (health.CurrentHealth / health.MaxHealth <= 0.5) return true;
        return false;
    }

    public void CheckFlip(Transform target)
    {
        int direction = 0;

        if (target.position.x > transform.position.x) direction = 1;
        else direction = -1;

        movement.FlipCheck(direction);
    }

    public void FireProjectile()
    {
        Projectile newNote = Instantiate(gardelStats.Projectiles[Random.Range(0, gardelStats.Projectiles.Length)], transform.position, Quaternion.identity);
        
        Vector2 direction = GameManager.Instance.PlayerInstance.transform.position - transform.position;

        newNote.LaunchProjectile(direction.normalized);
    }

    public void SpawnShout()
    {
        GameObject shout = Instantiate(gardelStats.ShoutAOEPrefab, transform.position, Quaternion.identity);

        Destroy(shout, 60/BeatManager.Instance.BPM);
    }

    public void StunningShout()
    {
        GameManager.Instance.PlayerInstance.TryToStunPlayer(gardelStats.StunEffectBeatDuration);
        CameraShakeManager.Instance.ScreenShakeFromProfile(gardelStats.StunShakeProfile, impulseSource);
    }

    public void StartSpeaking()
    {
        speaking = true;
    }

    public void StopSpeaking()
    {
        speaking=false;        
    }

    public float GetHealth()
    {
        return health.CurrentHealth;
    }

    #endregion
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnittaController : MonoBehaviour
{
    #region State Machine Varibles
    public StateMachine StateMachine { get; private set; }
    //public GardelST_Idle IdleState { get; private set; }
    //public GardelST_Jump JumpState { get; private set; }
    //public GardelST_Airborne AirborneState { get; private set; }
    //public GardelST_NormalAttack NormalAttackState { get; private set; }
    //public GardelST_SpecialAttack SpecialAttackState { get; private set; }
    //public GardelST_StunAttack StunAttackState { get; private set; }
    #endregion

    #region Component References
    public Core Core { get; private set; }

    private Core_Health health;
    private Core_Movement movement;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private CinemachineImpulseSource impulseSource;

    [Header("Scriptable Objects")]
    //[SerializeField] private GardelStats gardelStats;
    [SerializeField] private SoundLibraryObject soundLibrary;

    [field: Header("Boss Waypoints")]
    [field: SerializeField] public Transform[] Platforms {  get; private set; }

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

        Core.SetSoundLibrary(soundLibrary);

        health = Core.GetCoreComponent<Core_Health>();
        movement = Core.GetCoreComponent<Core_Movement>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
        soundLibrary.Initialize();

        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();

        StateMachine = new StateMachine();
        
    }

    private void OnEnable()
    {
        animatorEvent.OnAnimationFinishedTrigger += AnimationFinishedTrigger;
    }

    private void OnDisable()
    {
        

        animatorEvent.OnAnimationFinishedTrigger -= AnimationFinishedTrigger;
    }

    private void Update()
    {
        //StateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        //StateMachine.CurrentState.OnFixedUpdate();
    }

    #endregion

    #region Boss Attacks



    #endregion

    #region Other Functions

    public void PlaySound(string name)
    {
        SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound(name)).Play();
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

    public void StartSpeaking()
    {
        speaking = true;
    }

    public void StopSpeaking()
    {
        speaking = false;
    }

    public float GetHealth()
    {
        return health.CurrentHealth;
    }

    #endregion
}

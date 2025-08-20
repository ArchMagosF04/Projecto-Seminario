using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GardelController : MonoBehaviour
{
    #region State Machine Varibles
    public StateMachine StateMachine {  get; private set; }
    public GardelST_Idle IdleState { get; private set; }
    public GardelST_Jump JumpState { get; private set; }
    public GardelST_Airborne AirborneState { get; private set; }
    public GardelST_NormalAttack NormalAttackState { get; private set; }
    #endregion

    #region Component References
    public Core Core { get; private set; }
    
    private Core_Health health;
    private Core_Movement movement;
    private Core_CollisionSenses collisionSenses;
    private Animator anim;

    [SerializeField] private SoundLibraryObject soundLibrary;

    [field: Header("Boss Waypoints")]
    [field: SerializeField] public Transform rightPlatform { get; private set; }
    [field: SerializeField] public Transform leftPlatform { get; private set; }
    [field: SerializeField] public Transform stageCenter { get; private set; }

    [Header("Attack Prefabs")]
    [SerializeField] private Projectile[] projectiles;

    #endregion

    #region Other Variables
    [Header("Other")]
    public bool ExecutedDesiredAction { get; private set; } //Determines if the boss has decided on an attack to perform.
    public Transform DesiredJumpTarget;
    
    public enum ActionType { None, Normal, Special }
    public ActionType DesiredAction = ActionType.None;


    #endregion

    #region Unity Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        health = Core.GetCoreComponent<Core_Health>();
        movement = Core.GetCoreComponent<Core_Movement>();
        collisionSenses = Core.GetCoreComponent<Core_CollisionSenses>();

        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
        soundLibrary.Initialize();

        StateMachine = new StateMachine();
        IdleState = new GardelST_Idle(this, StateMachine, anim, "Idle");
        JumpState = new GardelST_Jump(this, StateMachine, anim, "InAir");
        AirborneState = new GardelST_Airborne(this, StateMachine, anim, "InAir");
        NormalAttackState = new GardelST_NormalAttack(this, StateMachine, anim, "NormalAttack");

        StateMachine.Initialize(IdleState);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        //transform.SetParent(null);
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

    public void OnBeatAction()
    {
        anim.SetTrigger("OnBeat");
        SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.Library["FingerSnap"]).Play();
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
        Projectile newNote = Instantiate(projectiles[Random.Range(0, projectiles.Length)], transform.position, Quaternion.identity);
        
        Vector2 direction = GameManager.Instance.PlayerTransform.position - transform.position;

        newNote.LaunchProjectile(direction.normalized);
    }

    #endregion
}

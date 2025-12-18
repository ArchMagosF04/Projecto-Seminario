using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamWeapon : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private enum BeamState { Idle, Aiming, Fire }

    private BeamState beamState = BeamState.Idle;

    private Vector3 target;

    public bool aimAtTarget;

    [SerializeField] private Vector2 defaultShootDirection = Vector2.down;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float timeToResetBeam = 0.2f;

    [Header("Weapon Stats")]

    [SerializeField] private float damage;
    [SerializeField] private float knockBack;

    [Header("AimMode Look")]
    [SerializeField] private float a_size = 0.1f;
    [SerializeField] private float a_startSize = 0.02f;


    [Header("FireMode Look")]
    [SerializeField] private float f_size = 0.35f;
    [SerializeField] private float f_startSize = 0.15f;

    [Header("Sounds")]
    [SerializeField] private SoundID aimSound;
    [SerializeField] private SoundID shootSound;


    private float startFireTime;

    private Vector3 beamDirection;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        timeToResetBeam = (60 / BeatManager.Instance.BPM) / 2;
    }

    private void Start()
    {
        lineRenderer.enabled = false;
        beamState = BeamState.Idle;
        SetBeamAimLook();
    }

    private void Update()
    {
        if (beamState != BeamState.Idle)
        {

            if (aimAtTarget) beamDirection = (target - transform.position);
            else beamDirection = defaultShootDirection;

            Draw2DRay(transform.position, transform.position - (-beamDirection.normalized * 20));

            if (beamState == BeamState.Fire && Time.time >= startFireTime + timeToResetBeam)
            {
                beamState = BeamState.Idle;
                lineRenderer.enabled = false;
                SetBeamAimLook();
            }
        }
    }

    public void SetAimWithTarget(Vector3 target)
    {
        SetAim();
        this.target = target;
    }

    public void SetAim()
    {
        lineRenderer.enabled = true;
        beamState = BeamState.Aiming;
        SetBeamAimLook();
        if (aimSound.IsValid()) BroAudio.Play(aimSound);
    }

    private void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    public void FireBeam()
    {
        beamState = BeamState.Fire;
        SetBeamAttackLook();

        if (shootSound.IsValid()) BroAudio.Play(shootSound);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, beamDirection.normalized, 40, targetMask);

        List<Collider2D> checkedColliders = new List<Collider2D>();

        if (hit)
        {
            if (checkedColliders.Contains(hit.collider)) return;

            bool addToList = false;

            if (hit.collider.TryGetComponent<Core_Health>(out Core_Health health))
            {
                health.TakeDamage(damage, beamDirection);
                addToList = true;
            }

            if (hit.collider.TryGetComponent<Core_Knockback>(out Core_Knockback knockback))
            {
                knockback.Knockback(transform, knockBack);
                addToList = true;
            }

            if (addToList) checkedColliders.Add(hit.collider);
        }

        startFireTime = Time.time;
    }

    private void SetBeamAttackLook()
    {
        lineRenderer.startWidth = f_startSize;
        lineRenderer.endWidth = f_size;
    }

    private void SetBeamAimLook()
    {
        lineRenderer.startWidth = a_startSize;
        lineRenderer.endWidth = a_size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position - (-beamDirection.normalized * 20));
    }
}

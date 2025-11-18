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

    [Header("FireMode Look")]
    [SerializeField] private float f_size = 0.35f;

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

        //Collider2D[] colliders = Physics2D.BoxCastAll(transform.position, newf_size, )

        RaycastHit2D hit = Physics2D.Raycast(transform.position, beamDirection.normalized, 40, targetMask);

        if (hit)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                health.TakeDamage(damage, beamDirection);
            }

            if (hit.collider.TryGetComponent<Core_Knockback>(out Core_Knockback knockback))
            {
                knockback.Knockback(transform, knockBack);
            }
        }

        startFireTime = Time.time;
    }

    private void SetBeamAttackLook()
    {
        lineRenderer.startWidth = f_size;
        lineRenderer.endWidth = f_size;
    }

    private void SetBeamAimLook()
    {
        lineRenderer.startWidth = a_size;
        lineRenderer.endWidth = a_size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, beamDirection * 40);
    }
}

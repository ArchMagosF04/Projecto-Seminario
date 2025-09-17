using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private float speed;

    [SerializeField] private int beatLifeTime = 3;
    [SerializeField, Range(0, 1f)] private float stopIntervalLength = 0.15f;

    [SerializeField] private ScreenShakeProfile shakeProfile;

    private Rigidbody2D rb;
    private CinemachineImpulseSource impulseSource;
    private Animator animator;

    private bool isMoving;
    private int beatTimer;
    private float stopIntervalStart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);

        stopIntervalLength *= 60/BeatManager.Instance.BPM;
    }

    private void OnEnable()
    {
        isMoving = false;
        beatTimer = 0;
        BeatManager.Instance.intervals[0].OnBeatEvent += OnBeatAction;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= OnBeatAction;
    }

    private void Update()
    {
        if (isMoving)
        {
            if (Time.time > stopIntervalStart + stopIntervalLength)
            {
                StopProjectile();
                isMoving = false;
            }
        }
    }

    public void OnBeatAction()
    {
        animator.ResetTrigger("OnBeat");
        animator.SetTrigger("OnBeat");

        if (!isMoving) beatTimer++;

        if (beatTimer > beatLifeTime)
        {
            Destroy(gameObject);
        }

        if (!isMoving)
        {
            LaunchProjectile(GetPlayerDirection());
            isMoving = true;

            stopIntervalStart = Time.time;
        }
    }

    public void LaunchProjectile(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    public void StopProjectile() => rb.velocity = Vector2.zero;

    public Vector2 GetPlayerDirection()
    {
        Vector2 dir = GameManager.Instance.PlayerInstance.transform.position - transform.position;

        return dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Core_Knockback component))
        {
            component.Knockback(transform, knockback);
            CameraShakeManager.Instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
        }

        if (collision.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(damage, transform.right);
        }

        Destroy(gameObject);
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveProjectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    private float speed;

    [SerializeField] private int beatLifeTime = 10;

    [SerializeField] private ScreenShakeProfile shakeProfile;

    [SerializeField] private float speedIncrement;
    [SerializeField] private float sizeIncrement;

    private Rigidbody2D rb;
    private CinemachineImpulseSource impulseSource;

    private int beatTimer;
    private int xDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        speed = 0;
        beatTimer = 0;
        BeatManager.Instance.intervals[0].OnBeatEvent += OnBeatAction;

        if (GameManager.Instance.PlayerInstance.transform.position.x > transform.position.x) xDirection = 1;
        else xDirection = -1;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= OnBeatAction;
    }

    public void OnBeatAction()
    {
        beatTimer++;

        if (beatTimer > beatLifeTime)
        {
            Destroy(gameObject);
        }

        speed += speedIncrement;
        LaunchProjectile();

        transform.localScale += new Vector3(0, sizeIncrement, 0);
    }

    public void LaunchProjectile()
    {
        rb.velocity = new Vector2(xDirection, 0) * speed;
    }

    public void StopProjectile() => rb.velocity = Vector2.zero;

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

        Debug.Log(collision.name);

        Destroy(gameObject);
    }
}

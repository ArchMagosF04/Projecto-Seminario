using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardelShoutAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce;

    [SerializeField] protected ScreenShakeProfile shakeProfile;

    private CinemachineImpulseSource impulseSource;

    private Animator anim;
    private CharacterAnimatorEvent characterAnimatorEvent;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        anim = GetComponent<Animator>();
        characterAnimatorEvent = GetComponent<CharacterAnimatorEvent>();

        anim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
    }

    private void OnEnable()
    {
        characterAnimatorEvent.OnAnimationFinishedTrigger += OnAttackEnd;
    }

    private void OnDisable()
    {
        characterAnimatorEvent.OnAnimationFinishedTrigger -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Core_Knockback component))
        {
            component.Knockback(transform, knockbackForce);

            CameraShakeManager.Instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
        }

        if (collision.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(damage, transform.right);
        }
    }
}

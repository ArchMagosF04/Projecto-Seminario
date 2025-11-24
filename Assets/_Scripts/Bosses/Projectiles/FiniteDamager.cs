using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class FiniteDamager : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce;

    [SerializeField] protected ScreenShakeProfile shakeProfile;

    private CinemachineImpulseSource impulseSource;

    [SerializeField] private bool pushInSetDirection = false;
    [SerializeField] private Vector2 setPushDirection;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hadEffect = false;

        if (collision.TryGetComponent(out Core_Knockback component))
        {
            if (!pushInSetDirection)
            {
                component.Knockback(transform, knockbackForce);
            }
            else
            {
                component.Knockback(setPushDirection, knockbackForce, 1);
            }

            if (!component.HyperArmor)
            {
                hadEffect = true;
                CameraShakeManager.Instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
            }
        }

        if (collision.TryGetComponent(out Core_Health health))
        {
            health.TakeDamage(damage, transform.right);
            if (!health.Invincible) hadEffect = true;
        }

        if (hadEffect) Destroy(gameObject);
    }
}

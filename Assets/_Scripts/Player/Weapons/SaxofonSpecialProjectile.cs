using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxofonSpecialProjectile : MonoBehaviour
{
    //[SerializeField] private float damage;
    //[SerializeField] private float knockback;
    //[SerializeField] private float speed;  
    
    //[SerializeField] private GameObject statusEffect;
    //[SerializeField] private float effectDuration;

    //private Vector2 moveDirection;

    //[SerializeField] private float lifeTime = 5f;

    //[SerializeField] private ScreenShakeProfile shakeProfile;

    //private Rigidbody2D rb;
    //private CinemachineImpulseSource impulseSource;
    //public bool destroy = false;

    //public event System.Action OnHit = delegate { };

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    impulseSource = GetComponent<CinemachineImpulseSource>();
    //}

    //private void Start()
    //{
    //    Destroy(gameObject, lifeTime);
    //}

    //public void LaunchProjectile(Vector2 direction)
    //{
    //    moveDirection = direction;
    //    rb.velocity = direction * speed;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent(out Core_Knockback component))
    //    {
    //        component.Knockback(transform, knockback);
    //        CameraShakeManager.Instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
    //    }

    //    if (collision.TryGetComponent(out Core_Health health))
    //    {
    //        health.TakeDamage(damage, transform.right);
    //        OnHit();
    //    }

    //    if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
    //    {
    //        if (statusEffect != null)
    //        {
    //            GameObject temp = null;
    //            temp = Instantiate<GameObject>(statusEffect, collision.transform);
    //            temp.GetComponent<doubleDamageStatus>().duration = effectDuration;
    //        }
    //        Destroy(gameObject);
    //    }

    //    if (destroy) Destroy(gameObject);

    //}

    //public void SetDamage(float amount)
    //{
    //    damage = amount;
    //}

    //public float GetSpeed()
    //{
    //    return speed;
    //}

    //public void SetSpeed(float amount)
    //{
    //    speed = amount;
    //}

    //public Vector2 GetDirection()
    //{
    //    return moveDirection;
    //}

    //private void OnDestroy()
    //{
    //    OnHit = null;
    //}
}

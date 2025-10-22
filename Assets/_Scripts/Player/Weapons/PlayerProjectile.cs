using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private bool isOnBeat;

    private float damage;
    private float speed;
    private float manaOnHit;

    private Rigidbody2D rb;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private Core_Mana manaComponent;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, lifetime);
    }

    private void OnEnable()
    {
        animatorEvent.OnAnimationFinishedTrigger += DestroyProjectile;
    }

    private void OnDisable()
    {
        animatorEvent.OnAnimationFinishedTrigger -= DestroyProjectile;
    }

    public void InitializeProjectile(bool isOnBeat, Core_Mana mana, float manaOnHit, float damage, float speed ,Vector2 direction)
    {
        this.isOnBeat = isOnBeat;
        manaComponent = mana;
        this.manaOnHit = manaOnHit;
        this.damage = damage;
        this.speed = speed;

        rb.velocity = direction * this.speed;

        if (!isOnBeat) spriteRenderer.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("OnHit", true);
        rb.velocity = Vector2.zero;

        if (collision.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(damage, transform.right);

            if (isOnBeat) manaComponent.IncreaseMana(manaOnHit);
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}

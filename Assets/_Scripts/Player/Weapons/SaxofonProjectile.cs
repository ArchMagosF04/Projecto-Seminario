using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxofonProjectile : MonoBehaviour
{
    private Vector2 moveDirection;

    [SerializeField] private int maxBounces = 2;

    [SerializeField] private ScreenShakeProfile shakeProfile;
    public bool destroy = false;

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
    private BeatComboCounter beatCombo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, lifetime);
    }

    public void LaunchProjectile(Vector2 direction)
    {
        moveDirection = direction;
        rb.velocity = direction * speed;
    }

    public void InitializeProjectile(bool isOnBeat, Core_Mana mana, float manaOnHit, float damage, float speed, Vector2 direction, BeatComboCounter beatCombo)
    {
        this.isOnBeat = isOnBeat;
        manaComponent = mana;
        this.manaOnHit = manaOnHit;
        this.damage = damage;
        this.speed = speed;
        this.beatCombo = beatCombo;

        moveDirection = direction;
        rb.velocity = direction * this.speed;

        if (!isOnBeat) spriteRenderer.color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (maxBounces <= 0) Destroy(gameObject);

        if ((collision.gameObject.layer == 3 || collision.gameObject.layer == 19))
        {
            LaunchProjectile(new Vector2 (moveDirection.x, moveDirection.y * -1));

            maxBounces--;
        }
        else if ((collision.gameObject.layer == 6))
        {
            LaunchProjectile(new Vector2(moveDirection.x * -1, moveDirection.y));

            maxBounces--;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(damage, transform.right);
            if (isOnBeat)
            {
                manaComponent.IncreaseMana(manaOnHit);
                beatCombo.IncreaseComboCounter();
            }
        }

        if (collision.gameObject.layer == 8) Destroy(gameObject);

        if (destroy) Destroy(gameObject);
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float amount)
    {
        speed = amount;
    }

    public Vector2 GetDirection()
    {
        return moveDirection;
    }
}

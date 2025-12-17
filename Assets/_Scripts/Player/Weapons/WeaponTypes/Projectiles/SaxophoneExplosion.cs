using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxophoneExplosion : MonoBehaviour
{
    [SerializeField] private int comboGainOnExplosion = 2;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask enemyMask;

    private float explosionDamage;
    private float manaOnHit;

    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private Core_Mana manaComponent;
    private SpriteRenderer spriteRenderer;
    private BeatComboCounter beatCombo;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        animatorEvent.OnAnimationFinishedTrigger += DestroyProjectile;
    }

    private void OnDisable()
    {
        animatorEvent.OnAnimationFinishedTrigger -= DestroyProjectile;
    }

    public void InitializeExplosion(Core_Mana mana, float manaOnHit, float explosionDamage, BeatComboCounter beatCombo)
    {
        manaComponent = mana;
        this.manaOnHit = manaOnHit;
        this.explosionDamage = explosionDamage;
        this.beatCombo = beatCombo;

        ExplosionCollision();
    }


    private void ExplosionCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyMask);

        foreach (Collider2D col in colliders)
        {
            if (col.TryGetComponent(out Core_Health damageable))
            {
                beatCombo.IncreaseComboCounter(comboGainOnExplosion);
                manaComponent.IncreaseMana(manaOnHit);

                Vector3 attackDirection = col.transform.position.x >= transform.position.x ? Vector3.right : Vector3.left;

                damageable.TakeDamage(explosionDamage * beatCombo.GetDamageMultiplier(), attackDirection);
            }
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

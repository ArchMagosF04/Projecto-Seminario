using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxophoneBullet : MonoBehaviour
{
    [SerializeField] private Color onBeatColor;
    [SerializeField] private Color onMissColor;

    [SerializeField] private SaxophoneExplosion explosionPrefab;
    [SerializeField] private SoundID explosionSound;

    private bool isOnBeat;

    private float projectileDamage;
    private float explosionDamage;

    private float speed;
    private float manaOnHit;

    private Rigidbody2D rb;
    private Animator anim;
    private CharacterAnimatorEvent animatorEvent;
    private Core_Mana manaComponent;
    private SpriteRenderer spriteRenderer;
    private BeatComboCounter beatCombo;

    private bool bombActivated;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bombActivated = false;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= SpawnExplosion;
    }

    public void InitializeProjectile(bool isOnBeat, Core_Mana mana, float manaOnHit, float hitDamage, float explosionDamage, float speed, Vector2 direction, BeatComboCounter beatCombo)
    {
        this.isOnBeat = isOnBeat;
        manaComponent = mana;
        this.manaOnHit = manaOnHit;
        this.projectileDamage = hitDamage;
        this.explosionDamage = explosionDamage;
        this.speed = speed;
        this.beatCombo = beatCombo;

        rb.velocity = direction * this.speed;

        spriteRenderer.color = isOnBeat? onBeatColor : onMissColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOnBeat)
        {
            if (collision.TryGetComponent(out Core_Health health))
            {
                health.TakeDamage(projectileDamage, transform.right);
            }

            DestroyProjectile();
        }
        else if(!bombActivated)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            bombActivated = true;
            transform.SetParent(collision.transform);

            BeatManager.Instance.intervals[0].OnBeatEvent += SpawnExplosion;
        }
    }

    public void SpawnExplosion()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= SpawnExplosion;

        SaxophoneExplosion newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        newExplosion.InitializeExplosion(manaComponent, manaOnHit, explosionDamage, beatCombo);

        if (explosionSound.IsValid()) BroAudio.Play(explosionSound);

        Destroy(gameObject);
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}

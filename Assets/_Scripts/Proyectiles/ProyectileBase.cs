using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileBase : MonoBehaviour
{
    private protected GameObject owner;
    private protected float damage;
    private protected float speed;
    [SerializeField] private protected float lifeSpan;
    private protected float currentLifeSpan;
    [SerializeField] private List<Sprite> sprites;

    Rigidbody2D rigidbody2d;


    protected virtual void Awake()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        rigidbody2d = this.gameObject.GetComponent<Rigidbody2D>();
        int temp = Random.Range(0, sprites.Count-1);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[temp];
    }
    void Start()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        rigidbody2d.velocity = transform.right * speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(currentLifeSpan < lifeSpan)
        {
            currentLifeSpan += Time.deltaTime;
        }
        else if (currentLifeSpan >= lifeSpan)
        {
            Destroy(this.gameObject);
        }
    }

    public void Initialize(float projectileDamage, GameObject shooter, float projectileSpeed = 3, float lifeTime = 5)
    {
        owner = shooter;
        damage = projectileDamage;
        speed = projectileSpeed;
        lifeSpan = lifeTime;
        if(shooter.layer == 10)
        {
            this.gameObject.layer = 12;
        }
        this.gameObject.GetComponent<Collider2D>().enabled = true;

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (owner != null && collision.gameObject != owner)
        {
            collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent health);
            if (health != null)
            {
                health.TakeDamage(damage);
            }

        }

        Destroy(this.gameObject);
    }
}

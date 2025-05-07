using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileBase : MonoBehaviour
{
    private GameObject owner;
    private float damage;
    private float speed;
    [SerializeField] private float lifeSpan;
    private float currentLifeSpan;

    Rigidbody2D rigidbody2d;


    private void Awake()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        rigidbody2d = this.gameObject.GetComponent<Rigidbody2D>();        
    }
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        rigidbody2d.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
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

    public void Initialize(float projectileDamage, GameObject shooter, float projectileSpeed = 3)
    {
        owner = shooter;
        damage = projectileDamage;
        speed = projectileSpeed;
        gameObject.layer = shooter.layer;
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
        if (collision.gameObject.layer != this.gameObject.layer)
        {
            if(this.gameObject.layer == 6 && collision.gameObject.layer != 3)
            {                
                Destroy(this.gameObject);
            }            
        }

        //if(collision.gameObject.layer == 3)
        //{
        //    Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.collider);
        //}        
    }
}

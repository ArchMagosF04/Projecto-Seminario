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
        if(shooter.layer == 6)
        {
            this.gameObject.layer = 7;
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

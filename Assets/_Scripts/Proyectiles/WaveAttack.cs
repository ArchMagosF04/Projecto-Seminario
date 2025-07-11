using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttack : ProyectileBase
{
    private float maxSize;
    private CircleCollider2D circleCollider;
    // Start is called before the first frame update

    protected override void Awake()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        circleCollider = GetComponent<CircleCollider2D>();
    }
    void Start()
    {
        
    }

    protected override void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    protected override void Update()
    {
        if(transform.localScale.magnitude < maxSize)
        {
            transform.localScale += new Vector3(speed, speed, speed) * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    public void Initialize(float projectileDamage, GameObject shooter, float maxWaveSize, float projectileSpeed = 3)
    {
        base.Initialize(projectileDamage,shooter,projectileSpeed);
        maxSize = maxWaveSize;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //leave empty
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            if (collision.gameObject.GetComponent<PlayerController>().CurrentState != collision.gameObject.GetComponent<PlayerController>().CurrentState as PlayerST_Jump)
            {
                collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent health);
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
            

        }        
    }
}

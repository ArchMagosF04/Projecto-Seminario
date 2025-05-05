using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileBase : MonoBehaviour
{
    private float damage;
    private GameObject owner;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignData(float projectileDamage, GameObject shooter)
    {
        damage = projectileDamage;
        owner = shooter;
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
    }
}

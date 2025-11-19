using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_DamageLess : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 moveDirection;

    [SerializeField] private float lifeTime = 5f;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void LaunchProjectile(Vector2 direction)
    {
        moveDirection = direction;
        rb.velocity = direction * speed;
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

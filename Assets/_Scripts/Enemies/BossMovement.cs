using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class BossMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D bossCollider;
    public EnemyStats enemyStats;
    [SerializeField] LayerMask ground;
    [SerializeField]private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    [SerializeField] private bool isGroundAbove;
    [SerializeField] private float fallingSpeedModifier;
    private float currentFallingSpeed = 1;
    [SerializeField] float transparancyDuration;
    [SerializeField]private float currentTransparancyDuration;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossCollider = gameObject.GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            currentFallingSpeed = currentFallingSpeed * fallingSpeedModifier;
            rb.AddForce(Vector2.down * currentFallingSpeed, ForceMode2D.Force);
        }

        //
        //if (isGrounded)
        //{
        //    Move(enemyStats.GroundAcceleration, enemyStats.GroundDeceleration, new Vector2(0, 0));
        //}
        //else
        //{
        //    Move(enemyStats.AirAcceleration, enemyStats.AirDeceleration, new Vector2(0, 0));
        //}
        //
    }

    private void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, ground);

        isGroundAbove = Physics2D.Raycast(transform.position, Vector2.up, 1f, ground);
    }
    public void BossJumpUp(Vector2 direction, float force)
    {
        if (isGrounded && bossCollider.enabled == true)
        {
            currentFallingSpeed = 1;
            Vector2 jumpDirection = direction * force;
            rb.AddForce(jumpDirection * force, ForceMode2D.Impulse);
        }
    }

    //
    public void Move(float acceleration, Vector2 moveInput)
    {
        rb.velocity = new Vector2(moveInput.x * acceleration, rb.velocity.y);
    }    
    //

}

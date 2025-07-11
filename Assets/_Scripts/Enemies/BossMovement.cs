using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class BossMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]Collider2D bossCollider;
    public EnemyStats enemyStats;
    [SerializeField] LayerMask ground;
    [SerializeField]private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    [SerializeField] private bool isGroundAbove;
    [SerializeField] private bool isGroundAbove2;
    [SerializeField] private float fallingSpeedModifier;
    private float currentFallingSpeed = 1;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float transparancyDuration;
    [SerializeField]private float currentTransparancyDuration;
    [SerializeField] Collider2D groundDetector;
    [SerializeField] float maxSpeed;

    [field: SerializeField] public Animator Anim { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //bossCollider = gameObject.GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (!isGrounded && rb.velocityY < maxFallSpeed)
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
        //isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, ground);        

        isGroundAbove = Physics2D.Raycast(new Vector2(transform.position.x-0.5f, transform.position.y), Vector2.up, 1f, ground);
        isGroundAbove2 = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.up, 1f, ground);

        if (!bossCollider.enabled)
        {
            if (currentTransparancyDuration < transparancyDuration)
            {
                currentTransparancyDuration += Time.deltaTime;
            }
            else if (currentTransparancyDuration >= transparancyDuration)
            {
                bossCollider.enabled = true;
                currentTransparancyDuration = 0;
            }
        }

        if (isGroundAbove || isGroundAbove2)
        {
            bossCollider.enabled = false;
        }
    }
    public void BossJumpUp(Vector2 direction, float force)
    {
        if (isGrounded && bossCollider.enabled == true)
        {
            currentFallingSpeed = 1;
            Vector2 jumpDirection = direction * force;
            rb.AddForce(jumpDirection * force, ForceMode2D.Impulse);
            Anim.SetTrigger("InAir");
        }
    }

    public void BossJump(Vector2 direction, float force)
    {
        if (isGrounded && bossCollider.enabled == true)
        {
            currentFallingSpeed = 1;
            Vector2 currentposition = transform.position;
            Vector2 jumpDirection = (direction - currentposition).normalized;
            rb.AddForce(new Vector2(jumpDirection.x * force, force), ForceMode2D.Impulse);
            Anim.SetTrigger("InAir");
        }
    }

    //public void Move(float acceleration, Vector2 moveInput)
    //{
    //    rb.velocity = new Vector2(moveInput.x * acceleration, rb.velocity.y);
    //}

    public void Move(float acceleration, Vector2 moveInput)
    {
        if(Mathf.Abs(rb.velocityX) < maxSpeed)
        {
            rb.AddForce(new Vector2(moveInput.x * acceleration, rb.velocity.y), ForceMode2D.Force);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            isGrounded = true;
            //Anim.ResetTrigger("InAir");
            Anim.SetTrigger("Idle");
        }       
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isGrounded = false;
            Anim.SetTrigger("InAir");
        }
       
    }    

}

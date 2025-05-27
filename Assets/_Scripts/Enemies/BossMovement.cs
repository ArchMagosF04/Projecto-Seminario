using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    #region Variables

    [Header("References")]
    public EnemyStats enemyStats;
    [SerializeField] private Collider2D _collider;

    private Rigidbody2D rb;
    //public Animator animator;

    //Movement Variables
    public float HorizontalVelocity { get; private set; }
    private bool _isFacingRight;

    //Collision Check Variables
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    [SerializeField] private bool _isGrounded;
    private bool _bumpedHead;

    //Jump Variables
    public float VerticalVelocity { get; private set; }
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isFastFalling;
    [SerializeField] private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;

    //Apex Variables
    private float _apexPoint;
    private float _timePastApexThreshold;
    [SerializeField] private bool _isPastApexThreshold;

    //State Machine Variables
    private StateMachine stateMachine;
    public Idle_Player IdleState { get; private set; }
    public Run_Player RunState { get; private set; }
    public Jump_Player JumpState { get; private set; }
    public Fall_Player FallState { get; private set; }
    public Dash_Player DashState { get; private set; }

    #endregion

    private void Awake()
    {
        _isFacingRight = true;

        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponentInChildren<Animator>();

        //stateMachine = new StateMachine();
        //IdleState = new Idle_Player(this, stateMachine);
        //RunState = new Run_Player(this, stateMachine);
        //JumpState = new Jump_Player(this, stateMachine);
        //FallState = new Fall_Player(this, stateMachine);
        //DashState = new Dash_Player(this, stateMachine);
    }

    private void Update()
    {
        JumpChecks();
        LandCheck();
        //AnimationChanges();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        //Jump();
        Fall();

        if (_isGrounded)
        {
            Move(enemyStats.GroundAcceleration, enemyStats.GroundDeceleration, new Vector2(0, 0));
        }
        else
        {
            Move(enemyStats.AirAcceleration, enemyStats.AirDeceleration, new Vector2(0, 0));
        }

        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        //Clamp fall speed
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -enemyStats.MaxFallSpeed, 50f);

        rb.velocity = new Vector2(HorizontalVelocity, VerticalVelocity);
    }

    #region Movement

    public void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput.x != enemyStats.MoveSpd)
        {
            TurnCheck(moveInput);

            float targetVelocity = 0f;

            targetVelocity = moveInput.x * enemyStats.MoveSpd;

            HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }

        else if (Mathf.Abs(moveInput.x) < enemyStats.MoveSpd)
        {
            HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, 0f, deceleration * Time.fixedDeltaTime);
        }

    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turn)
    {
        if (turn)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Land/Fall

    private void LandCheck()
    {
        //Landed
        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void Fall()
    {
        //Normal gravity while falling
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += enemyStats.Gravity * Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Jump

    private void ResetJumpValues()
    {
        _isJumping = false;
        _isFalling = false;
        _isFastFalling = false;
        _fastFallTime = 0f;
        _isPastApexThreshold = false;
    }

    private void JumpChecks()
    {
        //When we release the jump button
        if (_isJumping && VerticalVelocity > 0f)
        {
            if (_isPastApexThreshold)
            {
                _isPastApexThreshold = false;
                _isFastFalling = true;
                _fastFallTime = enemyStats.TimeForUpwardsCancel;
                VerticalVelocity = 0f;
            }
            else
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }
    } 


    //Initiate jump

    public void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        //_jumpBufferTimer = 0f;
        //_numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = enemyStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        //Apply gravity while jumping
        if (_isJumping)
        {
            //Check for head bump
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            //Gravity on Ascending
            if (VerticalVelocity >= 0f)
            {
                //Apex controls
                _apexPoint = Mathf.InverseLerp(enemyStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > enemyStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < enemyStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }

                //Gravity on ascending but not past apex threshold
                else if (!_isFastFalling)
                {
                    VerticalVelocity += enemyStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }

            //Gravity on descending
            else if (!_isFastFalling)
            {
                VerticalVelocity += enemyStats.Gravity * enemyStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        //Jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= enemyStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += enemyStats.Gravity * enemyStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < enemyStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / enemyStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Collision Checks

    private void IsGrounded()
        {
            Vector2 boxCastOrigin = new Vector2(_collider.bounds.center.x, _collider.bounds.min.y);
            Vector2 boxCastSize = new Vector2(_collider.bounds.size.x, enemyStats.GroundDetectionRayLength);

            _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, enemyStats.GroundDetectionRayLength, enemyStats.GroundLayer);

            if (_groundHit.collider != null)
            {
                _isGrounded = true;
            }
            else { _isGrounded = false; }



            //if (enemyStats.DebugShowIsGroundedBox)
            //{
            //    Color rayColor;
            //    if (_isGrounded)
            //    {
            //        rayColor = Color.green;
            //    }
            //    else { rayColor = Color.red; }

            //    Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * enemyStats.GroundDetectionRayLength, rayColor);
            //    Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * enemyStats.GroundDetectionRayLength, rayColor);
            //    Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - enemyStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
            //}
        }

        //private void BumpedHead()
        //{
        //    Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        //    Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * enemyStats.HeadWidth, enemyStats.HeadDetectionRayLength);

        //    _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, enemyStats.HeadDetectionRayLength, enemyStats.GroundLayer);

        //    if (_headHit.collider != null)
        //    {
        //        _bumpedHead = true;
        //    }
        //    else { _bumpedHead = false; }



        //    if (enemyStats.DebugShowHeadBumpBox)
        //    {
        //        float headWidth = enemyStats.HeadWidth;

        //        Color rayColor;
        //        if (_bumpedHead)
        //        {
        //            rayColor = Color.green;
        //        }
        //        else { rayColor = Color.red; }

        //        Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * enemyStats.HeadDetectionRayLength, rayColor);
        //        Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * enemyStats.HeadDetectionRayLength, rayColor);
        //        Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + enemyStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        //    }
        //}

        private void CollisionChecks()
        {
            IsGrounded();
            //BumpedHead();
        }

        #endregion

       

        //private void AnimationChanges()
        //{
        //    if (InputManager.Movement != Vector2.zero)
        //    {
        //        animator.SetBool("IsRunning", true);
        //        animator.SetBool("IsIdle", false);
        //    }
        //    else
        //    {
        //        animator.SetBool("IsIdle", true);
        //        animator.SetBool("IsRunning", false);
        //    }

        //    animator.SetBool("IsJumping", _isJumping);

        //    if (_isFalling || _isFastFalling || _isDashFastFalling)
        //    {
        //        animator.SetBool("IsFalling", true);
        //    }
        //    else
        //    {
        //        animator.SetBool("IsFalling", false);
        //    }

        //    if (_isDashing || _isAirDashing)
        //    {
        //        animator.SetBool("IsDashing", true);
        //    }
        //    else
        //    {
        //        animator.SetBool("IsDashing", false);
        //    }
        //}
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    #region Check Transforms
    [SerializeField] private Transform groundCheck;
    public Transform GroundCheck => groundCheck;

    #endregion

    [SerializeField] private float groundCheckRadius = 0.3f;
    public float GroundCheckRadius => groundCheckRadius;

    [SerializeField] private LayerMask whatIsGround;
    public LayerMask WhatIsGround => whatIsGround;

    #region Check Functions

    public bool Grounded
    {
        get => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    #endregion
}

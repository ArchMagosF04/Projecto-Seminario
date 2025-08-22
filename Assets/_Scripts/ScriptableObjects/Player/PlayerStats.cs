using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/BaseData")]
public class PlayerStats : ScriptableObject
{
    [Header("Move State")]
    [SerializeField] private float movementVelocity = 10f;

    public float MovementVelocity => movementVelocity;

    [Header("Jump State")]
    [SerializeField] private float jumpVelocity = 15f;
    [SerializeField] private int amountOfJumps = 1;

    public float JumpVelocity => jumpVelocity;
    public int AmountOfJumps => amountOfJumps;

    [Header("Airborne State")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f;

    public float CoyoteTime => coyoteTime;
    public float VariableJumpHeightMultiplier => variableJumpHeightMultiplier;

    [Header("Dash State")]
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashVelocity = 50f;
    [SerializeField] private float drag = 10f;
    [SerializeField] private float dashEndYMultipler = 0.2f;
    [SerializeField] private float dashCooldownReduction = 0.1f;
    //public float distBetweenAfterImages = 0.5f;

    public float DashCooldown => dashCooldown;
    public float DashTime => dashTime;
    public float DashVelocity => dashVelocity;
    public float Drag => drag;
    public float DashEndYMultiplier => dashEndYMultipler;
    public float DashCooldownReduction => dashCooldownReduction;

    [Header("Crouch State")]
    [SerializeField] private float crouchColliderHeight = 1.6f;
    [SerializeField] private float standColliderHeight = 2.6f;

    public float CrouchColliderHeight => crouchColliderHeight;
    public float StandColliderHeight => standColliderHeight;
}

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
    [SerializeField] private int manaGainOnDoubleJump = 2;

    public float JumpVelocity => jumpVelocity;
    public int AmountOfJumps => amountOfJumps;
    public int ManaGainOnDoubleJump => manaGainOnDoubleJump;

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
    [SerializeField] private int manaGainOnBeatDash = 2;

    public float DashCooldown => dashCooldown;
    public float DashTime => dashTime;
    public float DashVelocity => dashVelocity;
    public float Drag => drag;
    public float DashEndYMultiplier => dashEndYMultipler;
    public float DashCooldownReduction => dashCooldownReduction;
    public int ManaGainOnBeatDash => manaGainOnBeatDash;

    [Header("Crouch State")]
    [SerializeField] private float crouchPhysicsColliderHeight = 1.6f;
    [SerializeField] private float standPhysicsColliderHeight = 2.6f;
    [SerializeField] private float crouchDamageColliderHeight;
    [SerializeField] private float standDamageColliderHeight;

    public float CrouchPhysicsColliderHeight => crouchPhysicsColliderHeight;
    public float StandPhysicsColliderHeight => standPhysicsColliderHeight;
    public float CrouchDamageColliderHeight => crouchDamageColliderHeight;
    public float StandDamageColliderHeight => standDamageColliderHeight;
}

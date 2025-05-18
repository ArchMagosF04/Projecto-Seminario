using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;

    public Vector2 RawMovementInput { get; private set; }

    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set ; }

    public bool CrouchInput { get; private set; }
    public bool CrouchInputStop { get; private set; }

    public bool PrimaryAttackInput { get; private set; }
    public bool PrimaryAttackInputStop { get; private set; }
    public bool SecondaryAttackInput { get; private set; }
    public bool SecondaryAttackInputStop { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float primaryAttackInputStartTime;
    private float secondaryAttackInputStartTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        CheckPrimaryAttackInputHoldTime();
        CheckSecondaryAttackInputHoldTime();
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PrimaryAttackInput = true;
            PrimaryAttackInputStop = false;
            primaryAttackInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            PrimaryAttackInputStop = true;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SecondaryAttackInput = true;
            SecondaryAttackInputStop = false;
            secondaryAttackInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            SecondaryAttackInputStop = true;
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CrouchInput = true;
            CrouchInputStop = false;
        }
        else if (context.canceled)
        {
            CrouchInputStop = true;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop= true;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    public void UseCrouchInput() => CrouchInput = false;

    public void UsePrimaryAttackInput() => PrimaryAttackInput = false;
    public void UseSecondaryAttackInput() => SecondaryAttackInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }

    private void CheckPrimaryAttackInputHoldTime()
    {
        if (Time.time >= primaryAttackInputStartTime + inputHoldTime)
        {
            PrimaryAttackInput = false;
        }
    }

    private void CheckSecondaryAttackInputHoldTime()
    {
        if (Time.time >= secondaryAttackInputStartTime + inputHoldTime)
        {
            SecondaryAttackInput = false;
        }
    }
}

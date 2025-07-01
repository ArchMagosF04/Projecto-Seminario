using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;

        if (context.started)
        {
            PrimaryAttackInput = true;
            PrimaryAttackInputStop = false;
        }

        if (context.canceled)
        {
            PrimaryAttackInputStop = true;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;

        if (context.started)
        {
            SecondaryAttackInput = true;
            SecondaryAttackInputStop = false;
        }

        if (context.canceled)
        {
            SecondaryAttackInputStop = true;
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;

        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;

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
        if (GameManager.Instance.IsGamePaused) return;

        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;

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

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (context.started) GameManager.Instance.PauseMenu(!GameManager.Instance.IsGamePaused);
    }

    public void OnResetInput(InputAction.CallbackContext context)
    {
        //if (context.started) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;

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
}

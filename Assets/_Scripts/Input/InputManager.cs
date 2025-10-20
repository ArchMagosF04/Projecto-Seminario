using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    //Components
    private PlayerInput playerInput;

    #region Gameplay Input Data
    public Vector2 RawMovementInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; } = true;

    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; } = true;

    public bool CrouchInput { get; private set; }
    public bool CrouchInputStop { get; private set; } = true;

    public bool PrimaryAttackInput { get; private set; }
    public int PrimaryAttackInputStop { get; private set; } = 0;
    public bool SecondaryAttackInput { get; private set; }
    public bool SecondaryAttackInputStop { get; private set; } = true;

    public Action OnPrimaryAttackRelease;

    #endregion

    #region LevelSelector Input Data

    public Action OnMainMenu;
    public Action OnNextLevel;
    public Action OnPreviousLevel;
    public Action OnSelectLevel;
    public Action OnCloseWeaponScreen;

    #endregion

    #region Other Variables

    [SerializeField] private float inputHoldTime = 0.2f;
    [SerializeField] private float inputReleaseTime = 0.1f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float primaryAttackInputStopTime;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        CheckPrimaryAttackHoldTime();
    }

    #region Gameplay Action Map
    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PrimaryAttackInput = true;
            PrimaryAttackInputStop = 1;
        }

        if (context.canceled)
        {
            PrimaryAttackInputStop = 2;
            primaryAttackInputStopTime = Time.time;
            OnPrimaryAttackRelease?.Invoke();
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
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
            DashInputStop = true;
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

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PauseManager.Instance.OpenPauseScreen();
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    public void OnResetInput(InputAction.CallbackContext context)
    {
        if (context.started) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    public void UseCrouchInput() => CrouchInput = false;

    public void UsePrimaryAttackInput() => PrimaryAttackInput = false;
    public void UseSecondaryAttackInput() => SecondaryAttackInput = false;
    public void UsePrimaryAttackCharge() => PrimaryAttackInputStop = 0;

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

    private void CheckPrimaryAttackHoldTime()
    {
        if (PrimaryAttackInputStop != 2) return;

        if (Time.time >= primaryAttackInputStopTime + inputReleaseTime)
        {
            PrimaryAttackInputStop = 0;
        }
    }

    #endregion

    #region UI Action Map
    public void OnUnpauseInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        PauseManager.Instance.ResumeGame();
        playerInput.SwitchCurrentActionMap("Gameplay");
    }

    #endregion

    #region Level Selector Action Map

    public void OnMainMenuInput(InputAction.CallbackContext context)
    {
        if (context.started) OnMainMenu?.Invoke();
    }

    public void OnNextLevelInput(InputAction.CallbackContext context)
    {
        if (context.started) OnNextLevel?.Invoke();
    }

    public void OnPreviousLevelInput(InputAction.CallbackContext context)
    {
        if (context.started) OnPreviousLevel?.Invoke();
    }

    public void OnSelectLevelInput(InputAction.CallbackContext context)
    {
        if (context.started) OnSelectLevel?.Invoke();
    }

    public void OnCloseWeaponScreenInput(InputAction.CallbackContext context)
    {
        if (context.started) OnCloseWeaponScreen?.Invoke();
    }

    #endregion
}

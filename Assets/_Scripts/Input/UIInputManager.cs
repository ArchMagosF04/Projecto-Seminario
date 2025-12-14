using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInputManager : MonoBehaviour
{
    public static UIInputManager Instance;

    //Components
    private PlayerInput playerInput;

    private bool blockGoToPreviousMenu;
    private bool blockResumeGame;

    [SerializeField] private UnityEvent OnCancel;
    [SerializeField] private UnityEvent OnResume;

    //Game Inputs
    public bool CancelMenuInput { get; private set; }
    public bool ResumeInput { get; private set; }

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

    public void BlockGoingBackInMenu(bool input) => blockGoToPreviousMenu = input;

    public void BlockResumeGame(bool input) => blockResumeGame = input;

    public void DisableUIEscape()
    {
        BlockGoingBackInMenu(true);
        BlockResumeGame(true);
    }

    public void EnableUIEscape()
    {
        BlockGoingBackInMenu(false);
        BlockResumeGame(false);
    }

    public void OnCancelInput(InputAction.CallbackContext context)
    {
        if (context.started && !blockGoToPreviousMenu)
        {
            OnCancel?.Invoke();
            CancelMenuInput = true;
        }
        if (context.canceled) CancelMenuInput = false;
    }

    public void OnResumeInput(InputAction.CallbackContext context)
    {
        if (context.started && !blockResumeGame)
        {
            OnResume?.Invoke();
            ResumeInput = true;
        }
        if (context.canceled) ResumeInput = false;
    }

    public void UseCancelMenuInput() => CancelMenuInput = false;
    public void UseResumeInput() => ResumeInput = false;
}

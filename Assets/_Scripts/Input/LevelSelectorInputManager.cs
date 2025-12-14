using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LevelSelectorInputManager : MonoBehaviour
{
    public static LevelSelectorInputManager Instance;

    //Components
    private PlayerInput playerInput;

    [SerializeField] private UnityEvent OnNextLevel;
    [SerializeField] private UnityEvent OnPreviousLevel;
    [SerializeField] private UnityEvent OnWeaponsMenu;
    [SerializeField] private UnityEvent OnCloseMenu;
    [SerializeField] private UnityEvent OnMainMenu;

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

    public void OnNextLevelInput(InputAction.CallbackContext context)
    {
        if (context.started) OnNextLevel?.Invoke();
    }

    public void OnPreviousLevelInput(InputAction.CallbackContext context)
    {
        if (context.started) OnPreviousLevel?.Invoke();
    }

    public void OnWeaponsMenuInput(InputAction.CallbackContext context)
    {
        if (context.started) OnWeaponsMenu?.Invoke();
    }

    public void OnCloseMenuInput(InputAction.CallbackContext context)
    {
        if (context.started) OnCloseMenu?.Invoke();
    }

    public void OnMainMenuInput(InputAction.CallbackContext context)
    {
        if (context.started) OnMainMenu?.Invoke();
    }
}

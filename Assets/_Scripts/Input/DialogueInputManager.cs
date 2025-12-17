using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueInputManager : MonoBehaviour
{
    public static DialogueInputManager Instance;

    //Components
    private PlayerInput playerInput;

    //Game Inputs
    public bool NextDialogueInput { get; private set; }
    public bool SkipDialogueInput { get; private set; }

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

    public void OnNextDialogueInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            NextDialogueInput = true;
        }
        if (context.canceled) NextDialogueInput = false;
    }

    public void OnSkipDialogueInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SkipDialogueInput = true;
        }
        if (context.canceled) SkipDialogueInput = false;
    }

    public void UseNextDialogueInput() => NextDialogueInput = false;
    public void UseSkipDialogueInput() => SkipDialogueInput = false;
}

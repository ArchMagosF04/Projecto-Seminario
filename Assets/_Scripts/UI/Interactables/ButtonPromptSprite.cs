using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPromptSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer imageComponent;

    [Header("Prompts Icons")]
    [SerializeField] private Sprite gamepadPrompt;
    [SerializeField] private Sprite keyboardMousePrompt;

    private void Awake()
    {
        imageComponent = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (Gamepad.all.Count > 0) OnGamepadConnected();
        else OnGamepadNotConnected();
    }

    [ContextMenu("Switch To Gamepad Icon")]
    public void OnGamepadConnected()
    {
        imageComponent.sprite = gamepadPrompt;
    }

    [ContextMenu("Switch To Keyboard & Mouse Icon")]
    public void OnGamepadNotConnected()
    {
        imageComponent.sprite = keyboardMousePrompt;
    }
}

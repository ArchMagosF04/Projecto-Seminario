using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsSettings : MonoBehaviour
{
    [Header("Keyboard & Mouse")]
    [SerializeField] private GameObject keyboardMousePanel;
    [SerializeField] private Selectable kMFirstSelected;
    [SerializeField] private string keyboardMouseNameScheme;

    [Header("Gamepad")]
    [SerializeField] private GameObject gamepadPanel;
    [SerializeField] private Selectable gPFirstSelected;
    [SerializeField] private string gamepadNameScheme;

    [Header("Other")]
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();
    }

    public void OnControlsScreenOpened()
    {
        if (Gamepad.all.Count > 0)
        {
            keyboardMousePanel.SetActive(false);
            gamepadPanel.SetActive(true);
            eventSystem.SetSelectedGameObject(gPFirstSelected.gameObject);
        }
        else
        {
            gamepadPanel.SetActive(false);
            keyboardMousePanel.SetActive(true);
            eventSystem.SetSelectedGameObject(kMFirstSelected.gameObject);
        }
    }

    public void ResetKeyboardAndMouseBindings()
    {
        foreach (InputActionMap map in inputActionAsset.actionMaps)
        {
            foreach(InputAction action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(keyboardMouseNameScheme));
            }
        }
    }

    public void ResetGamepadBindings()
    {
        foreach (InputActionMap map in inputActionAsset.actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(gamepadNameScheme));
            }
        }
    }
}

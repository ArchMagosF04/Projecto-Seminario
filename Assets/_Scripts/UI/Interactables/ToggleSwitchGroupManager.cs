using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    [Header("Start Value")]
    [SerializeField] private SwitchToggle initialToggle;

    [Header("Toggle Options")]
    [SerializeField] private bool allCanBeToggleOff;

    private List<SwitchToggle> toggles = new List<SwitchToggle>();

    private void Awake()
    {
        SwitchToggle[] toggleSwitches = GetComponentsInChildren<SwitchToggle>();
        foreach (var toggle in toggleSwitches)
        {
            RegisterToggleButtonToGroup(toggle);
        }
    }

    private void RegisterToggleButtonToGroup(SwitchToggle toggle)
    {
        if (toggles.Contains(toggle)) return;

        toggles.Add(toggle);

        toggle.SetupForManager(this);
    }

    private void Start()
    {
        bool areAllToggleOff = true;

        foreach (var button in toggles)
        {
            if (!button.CurrentValue) continue;

            areAllToggleOff = false;
            break;
        }

        if (!areAllToggleOff || allCanBeToggleOff) return;

        if (initialToggle != null) initialToggle.ToggleByGroupManager(true);
        else toggles[0].ToggleByGroupManager(true);
    }

    public void ToggleGroup(SwitchToggle toggleSwitch)
    {
        if (toggles.Count <= 1) return;

        if (allCanBeToggleOff && toggleSwitch.CurrentValue)
        {
            foreach(var button in toggles)
            {
                if (button == null) continue;

                button.ToggleByGroupManager(false);
            }
        }
        else
        {
            foreach (var button in toggles)
            {
                if (button == null) continue;

                if (button == toggleSwitch) button.ToggleByGroupManager(true);
                else button.ToggleByGroupManager(false);
            }
        }
    }
}

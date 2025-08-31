using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;


    private void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0) vSyncToggle.isOn = false;
        else vSyncToggle.isOn = true;

        SetUpResolutionDropdown();
    }

    private void SetUpResolutionDropdown()
    {
        availableResolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resOptions = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            //if (availableResolutions[i].refreshRateRatio.value != 60 && availableResolutions[i].refreshRateRatio.value != 30 && availableResolutions[i].refreshRateRatio.value != 120 && availableResolutions[i].refreshRateRatio.value != 144)
            //    return;

            string option = $"{availableResolutions[i].width}X{availableResolutions[i].height} @{availableResolutions[i].refreshRateRatio}hz";
            resOptions.Add(option);

            if (availableResolutions[i].width == Screen.currentResolution.width 
                && availableResolutions[i].height == Screen.currentResolution.height) currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = availableResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ApplyVideoSettings()
    {
        Screen.fullScreen = fullscreenToggle.isOn;

        if (vSyncToggle.isOn) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }
}
